using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target; // �÷��̾� ĳ���� ����
    public float cameraHeight = 15f; // ī�޶� ���� ����
    public Vector3 cameraOffset = new Vector3(0, 8, 0); // ī�޶��� ������
    public Vector2 cameraRotation = new Vector3(90,0,0);
    private void LateUpdate()
    {
        if (target != null)
        {
            // �÷��̾� ��ġ�� �������� ī�޶� ��ġ ����
            Vector3 targetPosition = target.position + cameraOffset;
            transform.position = targetPosition;
            transform.rotation = Quaternion.Euler(cameraRotation);
        }
    }
}
