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

    private Rigidbody _rigidbody;
    private Animator animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // 카메라 설정
        if (topDownCamera != null)
        {
            topDownCamera.transform.position = transform.position + Vector3.up * cameraHeight + cameraOffset;
            topDownCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // 탑다운 시점으로 회전
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
        Vector3 newPosition = _rigidbody.position + movement * moveSpeed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(newPosition);
    }
}
