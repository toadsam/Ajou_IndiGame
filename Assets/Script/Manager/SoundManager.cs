using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;

    [Header("Scene BGM Mapping")]
    public AudioClip defaultBGM; // �⺻ BGM
    public SceneBGM[] sceneBGMMappings; // �� �̸��� BGM ����

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBGMForScene(SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(scene.name);
    }

    public void PlayBGMForScene(string sceneName)
    {
        AudioClip clip = GetBGMForScene(sceneName);
        PlayBGM(clip);
    }

    private AudioClip GetBGMForScene(string sceneName)
    {
        foreach (var mapping in sceneBGMMappings)
        {
            if (mapping.sceneName == sceneName)
                return mapping.bgmClip;
        }
        return defaultBGM; // ������ ������ �⺻ BGM ���
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip)
            return;

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}

[System.Serializable]
public class SceneBGM
{
    public string sceneName; // �� �̸�
    public AudioClip bgmClip; // ���� �´� BGM
}
