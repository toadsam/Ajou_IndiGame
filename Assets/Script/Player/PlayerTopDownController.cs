using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopDownController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // 플레이어 이동 속도
    private Vector3 movement;

    private Rigidbody _rigidbody;
    private Animator animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 입력값 받기 (WASD 또는 방향키)
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // 입력값에 따라 이동 방향 설정
        movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // 걷기 애니메이션 설정 및 방향 회전
        if (movement != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
            RotateTowardsMovementDirection(); // 이동 방향으로 즉각 회전
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
        // Rigidbody를 이용해 이동 처리
        Vector3 newPosition = _rigidbody.position + movement * moveSpeed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(newPosition);
    }

    private void RotateTowardsMovementDirection()
    {
        // 이동 방향이 0이 아닐 때만 회전 처리
        if (movement != Vector3.zero)
        {
            // 입력된 방향을 기준으로 캐릭터가 해당 방향을 바라보도록 설정
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = targetRotation;
        }
    }
}
