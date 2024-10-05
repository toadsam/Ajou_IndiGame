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

    private Rigidbody _rigidbody;
    private Animator animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // ī�޶� ����
        if (topDownCamera != null)
        {
            topDownCamera.transform.position = transform.position + Vector3.up * cameraHeight + cameraOffset;
            topDownCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // ž�ٿ� �������� ȸ��
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
        Vector3 newPosition = _rigidbody.position + movement * moveSpeed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(newPosition);
    }
}
