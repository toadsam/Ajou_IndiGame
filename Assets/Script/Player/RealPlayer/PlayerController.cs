using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpForce;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;
    public bool playerEventOff = true;

    private Rigidbody _rigidbody;
   // private AudioSource _audioSource;

    //public AudioClip footstepClip;
    public float footstepInterval = 0.5f;
    private float footstepTimer;

    private float startSpeed;

    public static bool isMove;

    private Animator animator;

    [Header("Attack")]
    public Collider attackCollider; // 공격 범위로 사용할 Trigger Collider
    public bool isAttacking = false; // 공격 중인지 여부

    public static PlayerController instance;
    private void Awake()
    {
        isMove = true;
        instance = this;
        _rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Animator 컴포넌트 참조
                                             //  _audioSource = GetComponent<AudioSource>();
        attackCollider.enabled = false; // 처음에는 공격 범위를 비활성화
    }

    void Start()
    {
        startSpeed = moveSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        footstepTimer = footstepInterval;
    }
    private void Update()
    {
        // 걷는 상태로 전환
        if (curMovementInput != Vector2.zero)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        // 달리기 상태 (Shift 키를 누를 때)
        if (Keyboard.current.leftShiftKey.isPressed)
        {
            animator.SetBool("isRunning", true);
            moveSpeed = 2 * startSpeed;
        }
        else
        {
            animator.SetBool("isRunning", false);
            moveSpeed = startSpeed;
        }
    }

    private void FixedUpdate()
    {
        
            Move();
    }

    private void LateUpdate()
    {
        //&& WakeUp.isWakeUp
       
            CameraLook();
        
    }

    public static void IsMove(bool changeMove)  //이걸 통해서 바꾸면 된다.
    {
        isMove = changeMove;
    }

    public void TogglePlayerInput(bool enable)
    {
        canLook = enable;
        isMove = enable;
        if (!enable)
        {
            // 움직임 멈추기
            curMovementInput = Vector2.zero;
            _rigidbody.velocity = Vector3.zero;
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
        //Debug.Log("여기에 들어오나");
       // HandleFootsteps();
    }

    void HandleFootsteps()
    {
        if (curMovementInput != Vector2.zero)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0)
            {
             //   _audioSource.PlayOneShot(footstepClip);
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0;
        }
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (IsGrounded())
            {
                _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
                animator.SetTrigger("isJumping"); // 점프 애니메이션 트리거
            }
        }
    }



    public void OnSensitivityIncrease(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            lookSensitivity += 0.05f;
            Debug.Log("������ ����" + lookSensitivity);
        }


    }

    public void OnSensitivityDecrease(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            lookSensitivity -= 0.05f;
            Debug.Log("������ ����" + lookSensitivity);
        }

    }

    // 마우스 좌클릭 시 공격 애니메이션 실행
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
           // Debug.Log("Attack triggered");
            animator.SetTrigger("isAttacking"); // 공격 애니메이션 트리거
            moveSpeed = 0;
        }
    }

    // 애니메이션 이벤트: 공격 시작 시 호출
    public void StartAttack()
    {
       // Debug.Log("Attack started.");
       isAttacking = true;
        attackCollider.enabled = true; // 공격 범위 활성화
    }

    // 애니메이션 이벤트: 공격 종료 시 호출
    public void EndAttack()
    {
        //Debug.Log("Attack ended.");
        attackCollider.enabled = false; // 공격 범위 비활성화
        isAttacking = false;
    }


    private bool IsGrounded()
    {
       
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.01f) , Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f)+ (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            Debug.DrawRay(rays[i].origin, rays[i].direction * 0.1f, Color.red); // 레이 길이를 1.0f로 설정해 시각적으로 확인
            if (Physics.Raycast(rays[i], 3.0f, groundLayerMask))
            {
                
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(isAttacking);
        if (isAttacking && other.CompareTag("Monster"))
        {
            Debug.Log("일로 들어옴?");
            MonsterStats monster = other.GetComponent<MonsterStats>();
            if (monster != null)
            {
                int attackDamage = PlayerStats.instance.strength;
                monster.TakeDamage(attackDamage);
                Debug.Log("Dealt " + attackDamage + " damage to the monster.");
            }
        }
    }

}
