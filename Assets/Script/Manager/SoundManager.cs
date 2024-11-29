using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;

    [Header("Scene BGM Mapping")]
    public AudioClip defaultBGM; // 기본 BGM
    public SceneBGM[] sceneBGMMappings; // 씬 이름과 BGM 매핑

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
        return defaultBGM; // 매핑이 없으면 기본 BGM 재생
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
    public string sceneName; // 씬 이름
    public AudioClip bgmClip; // 씬에 맞는 BGM
}
