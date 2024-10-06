using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target; // 플레이어 캐릭터 참조
    public float cameraHeight = 15f; // 카메라 높이 설정
    public Vector3 cameraOffset = new Vector3(0, 8, 0); // 카메라의 오프셋
    public Vector2 cameraRotation = new Vector3(90,0,0);
    private void LateUpdate()
    {
        if (target != null)
        {
            // 플레이어 위치를 기준으로 카메라 위치 설정
            Vector3 targetPosition = target.position + cameraOffset;
            transform.position = targetPosition;
            transform.rotation = Quaternion.Euler(cameraRotation);
        }
    }
}
