using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopDownController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // �÷��̾� �̵� �ӵ�
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
        // �Է°� �ޱ� (WASD �Ǵ� ����Ű)
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // �Է°��� ���� �̵� ���� ����
        movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // �ȱ� �ִϸ��̼� ���� �� ���� ȸ��
        if (movement != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
            RotateTowardsMovementDirection(); // �̵� �������� �ﰢ ȸ��
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void FixedUpdate()
    {
        // �÷��̾� �̵�
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Rigidbody�� �̿��� �̵� ó��
        Vector3 newPosition = _rigidbody.position + movement * moveSpeed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(newPosition);
    }

    private void RotateTowardsMovementDirection()
    {
        // �̵� ������ 0�� �ƴ� ���� ȸ�� ó��
        if (movement != Vector3.zero)
        {
            // �Էµ� ������ �������� ĳ���Ͱ� �ش� ������ �ٶ󺸵��� ����
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = targetRotation;
        }
    }
}
