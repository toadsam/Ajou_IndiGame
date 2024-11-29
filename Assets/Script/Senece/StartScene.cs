using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public string sceneToLoad; // 이동할 씬 이름을 에디터에서 설정할 수 있도록 변수 선언
    public Button startButton; // 버튼을 에디터에서 할당

    [Header("Audio Settings")]
    public AudioSource audioSource; // BGM 재생을 위한 AudioSource
    public AudioClip startSceneMusic; // 시작 씬 음악

    private void Start()
    {
        // 버튼 클릭 이벤트에 OnStartButtonClicked 메서드를 등록
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        else
        {
            Debug.LogWarning("Start 버튼이 할당되지 않았습니다.");
        }

        // 씬 시작 시 바로 오디오 재생
        if (audioSource != null && startSceneMusic != null)
        {
            audioSource.clip = startSceneMusic;
            audioSource.loop = true; // 음악 반복 재생
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource 또는 음악이 설정되지 않았습니다.");
        }
    }

    private void OnStartButtonClicked()
    {
        // 씬 전환 시 음악 중지
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad); // 설정한 씬 이름으로 씬 이동
        }
        else
        {
            Debug.LogWarning("이동할 씬 이름이 설정되지 않았습니다.");
        }
    }
}
