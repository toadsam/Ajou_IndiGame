using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopDownController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // �÷��̾� �̵� �ӵ�
    private Vector3 movement;

    [Header("Camera Settings")]
    public Camera topDownCamera; // ž�ٿ� ī�޶� ����
    public float cameraHeight = 15f; // ī�޶� ���� ����
    public Vector3 cameraOffset; // ī�޶� ������ ����
    public float cameraTiltAngle = 10f; // ī�޶� �������� ����

    private Rigidbody _rigidbody;
    private Animator animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // ī�޶� ����: �ణ ��Ʋ���� ���� ����
        if (topDownCamera != null)
        {
            Vector3 cameraPosition = transform.position + Vector3.up * cameraHeight + cameraOffset;
            topDownCamera.transform.position = cameraPosition;

            // ī�޶��� ȸ�� ������ �����Ͽ� ���鵵 ���̵��� ����
            topDownCamera.transform.rotation = Quaternion.Euler(cameraTiltAngle, 0f, 0f);
        }
    }

    void Update()
    {
        // �Է°� �ޱ� (WASD �Ǵ� ����Ű)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movement = new Vector3(horizontalInput, 0f, verticalInput);

        // �ȱ� �ִϸ��̼� ����
        if (movement != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
            // �̵� �������� ĳ���� ȸ��
            RotateTowardsMovementDirection();
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
        Vector3 newPosition = _rigidbody.position + movement.normalized * moveSpeed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(newPosition);
    }

    private void RotateTowardsMovementDirection()
    {
        // �̵� ������ 0�� �ƴ� ���� ȸ�� ó��
        if (movement != Vector3.zero)
        {
            // �̵� �������� ȸ���ϵ��� ����
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
