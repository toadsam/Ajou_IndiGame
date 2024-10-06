using UnityEngine;

public class Portal : MonoBehaviour
{
    public string portalName; // 포탈 이름을 설정하여 이동할 씬 이름을 결정

    private void OnTriggerEnter(Collider other)
    {
        // 포탈과 닿은 객체가 플레이어일 때만 처리
        if (other.CompareTag("Player"))
        {
            // SceneLoader 싱글톤을 통해 씬 로드 요청
            if (SceneLoader.instance != null)
            {
                SceneLoader.instance.LoadSceneBasedOnPortalName(portalName);
            }
            else
            {
                Debug.LogWarning("SceneLoader 인스턴스가 존재하지 않습니다.");
            }
        }
    }
}
