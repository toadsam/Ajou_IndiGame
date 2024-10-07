using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target; // �÷��̾� ĳ���� ����
    public float cameraHeight = 15f; // ī�޶� ���� ����
    public Vector3 cameraOffset = new Vector3(0, 8, -5); // ī�޶��� ������
    public Vector3 cameraRotation = new Vector3(60, 0, 0); // x, y, z ���� ����

    private void LateUpdate()
    {
        if (target != null)
        {
            // �÷��̾� ��ġ�� �������� ī�޶� ��ġ ����
            Vector3 targetPosition = target.position + cameraOffset;
            transform.position = targetPosition;

            // ī�޶� ȸ���� �񽺵��� ���� (�ణ ������ ���̵���)
            transform.rotation = Quaternion.Euler(cameraRotation);
        }
    }
}
