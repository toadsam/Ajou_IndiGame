using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopDownController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // 플레이어 이동 속도
    private Vector3 movement;

    [Header("Camera Settings")]
    public Camera topDownCamera; // 탑다운 카메라 참조
    public float cameraHeight = 15f; // 카메라 높이 설정
    public Vector3 cameraOffset; // 카메라 오프셋 설정
    public float cameraTiltAngle = 10f; // 카메라가 기울어지는 각도

    private Rigidbody _rigidbody;
    private Animator animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // 카메라 설정: 약간 비틀어진 각도 설정
        if (topDownCamera != null)
        {
            Vector3 cameraPosition = transform.position + Vector3.up * cameraHeight + cameraOffset;
            topDownCamera.transform.position = cameraPosition;

            // 카메라의 회전 각도를 조정하여 측면도 보이도록 설정
            topDownCamera.transform.rotation = Quaternion.Euler(cameraTiltAngle, 0f, 0f);
        }
    }

    void Update()
    {
        // 입력값 받기 (WASD 또는 방향키)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movement = new Vector3(horizontalInput, 0f, verticalInput);

        // 걷기 애니메이션 설정
        if (movement != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
            // 이동 방향으로 캐릭터 회전
            RotateTowardsMovementDirection();
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void FixedUpdate()
    {
        // 플레이어 이동
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 newPosition = _rigidbody.position + movement.normalized * moveSpeed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(newPosition);
    }

    private void RotateTowardsMovementDirection()
    {
        // 이동 방향이 0이 아닐 때만 회전 처리
        if (movement != Vector3.zero)
        {
            // 이동 방향으로 회전하도록 설정
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
