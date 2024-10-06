using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static SceneLoader instance;

    private void Awake()
    {
        // 싱글톤 설정: 이미 인스턴스가 존재하면 파괴, 그렇지 않으면 유지
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
        }
    }

    // 씬 로드 및 리스폰 포인트로 플레이어 이동 처리
    public void LoadSceneBasedOnPortalName(string portalName)
    {
        string sceneName = "";

        switch (portalName)
        {
            case "SeonghoHallPortal":
                sceneName = "SeonghoHallScene";
                break;
            case "WoncheonHallPortal":
                sceneName = "WoncheonHallScene";
                break;
            case "PalDalHallPortal":
                sceneName = "PalDalHallScene";
                break;
            case "SanhakHallPortal":
                sceneName = "SanhakHallScene";
                break;
            default:
                Debug.LogWarning("포탈 이름이 잘못되었습니다: " + portalName);
                return;
        }

        // 씬 로드
        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드 후 리스폰 처리
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 리스폰 포인트 오브젝트를 찾아서 플레이어를 그 위치로 이동
        GameObject respawnPoint = GameObject.FindWithTag("RespawnPoint");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && respawnPoint != null)
        {
            player.transform.position = respawnPoint.transform.position;
            Debug.Log("여기에 들어왔습니다");
        }

        // 이벤트 핸들러 해제 (중복 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
