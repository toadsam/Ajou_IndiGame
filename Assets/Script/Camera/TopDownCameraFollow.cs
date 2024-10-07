using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target; // 플레이어 캐릭터 참조
    public float cameraHeight = 15f; // 카메라 높이 설정
    public Vector3 cameraOffset = new Vector3(0, 8, -5); // 카메라의 오프셋
    public Vector3 cameraRotation = new Vector3(60, 0, 0); // x, y, z 각도 설정

    private void LateUpdate()
    {
        if (target != null)
        {
            // 플레이어 위치를 기준으로 카메라 위치 설정
            Vector3 targetPosition = target.position + cameraOffset;
            transform.position = targetPosition;

            // 카메라 회전을 비스듬히 설정 (약간 측면이 보이도록)
            transform.rotation = Quaternion.Euler(cameraRotation);
        }
    }
}
