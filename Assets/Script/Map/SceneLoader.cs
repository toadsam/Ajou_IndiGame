using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static SceneLoader instance;

    private void Awake()
    {
        // �̱��� ����: �̹� �ν��Ͻ��� �����ϸ� �ı�, �׷��� ������ ����
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �ı����� �ʵ��� ����
        }
    }

    // �� �ε� �� ������ ����Ʈ�� �÷��̾� �̵� ó��
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
                Debug.LogWarning("��Ż �̸��� �߸��Ǿ����ϴ�: " + portalName);
                return;
        }

        // �� �ε�
        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += OnSceneLoaded; // �� �ε� �� ������ ó��
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ������ ����Ʈ ������Ʈ�� ã�Ƽ� �÷��̾ �� ��ġ�� �̵�
        GameObject respawnPoint = GameObject.FindWithTag("RespawnPoint");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && respawnPoint != null)
        {
            player.transform.position = respawnPoint.transform.position;
            Debug.Log("���⿡ ���Խ��ϴ�");
        }

        // �̺�Ʈ �ڵ鷯 ���� (�ߺ� ����)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
