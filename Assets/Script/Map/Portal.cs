using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필요

public class Portal : MonoBehaviour
{
    // 포탈 이름에 따라 이동할 씬 이름을 결정하는 변수
    public string portalName; // 이 포탈에 설정된 이름으로 씬을 구분

    // 플레이어가 포탈에 닿았을 때 호출되는 메소드
    private void OnTriggerEnter(Collider other)
    {
        // 포탈과 닿은 객체가 플레이어일 때만 처리
        if (other.CompareTag("Player"))
        {
            LoadSceneBasedOnPortalName();
        }
    }

    // 포탈 이름에 따라 씬을 로드하는 함수
    private void LoadSceneBasedOnPortalName()
    {
        switch (portalName)
        {
            case "SeonghoHallPortal":
                SceneManager.LoadScene("SeonghoHallScene"); // 성호관 씬으로 이동
                break;
            case "WoncheonHallPortal":
                SceneManager.LoadScene("WoncheonHallScene"); // 원천관 씬으로 이동
                break;
            case "PalDalHallPortal":
                SceneManager.LoadScene("PalDalHallScene"); // 팔달관 씬으로 이동
                break;
            case "SanhakHallPortal":
                SceneManager.LoadScene("SanhakHallScene"); // 산학원 씬으로 이동
                break;
            default:
                Debug.LogWarning("포탈 이름이 잘못되었습니다: " + portalName);
                break;
        }
    }
}
