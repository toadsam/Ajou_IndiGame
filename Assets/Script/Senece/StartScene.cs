using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public string sceneToLoad; // �̵��� �� �̸��� �����Ϳ��� ������ �� �ֵ��� ���� ����
    public Button startButton; // ��ư�� �����Ϳ��� �Ҵ�

    [Header("Audio Settings")]
    public AudioSource audioSource; // BGM ����� ���� AudioSource
    public AudioClip startSceneMusic; // ���� �� ����

    private void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ�� OnStartButtonClicked �޼��带 ���
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        else
        {
            Debug.LogWarning("Start ��ư�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        // �� ���� �� �ٷ� ����� ���
        if (audioSource != null && startSceneMusic != null)
        {
            audioSource.clip = startSceneMusic;
            audioSource.loop = true; // ���� �ݺ� ���
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource �Ǵ� ������ �������� �ʾҽ��ϴ�.");
        }
    }

    private void OnStartButtonClicked()
    {
        // �� ��ȯ �� ���� ����
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad); // ������ �� �̸����� �� �̵�
        }
        else
        {
            Debug.LogWarning("�̵��� �� �̸��� �������� �ʾҽ��ϴ�.");
        }
    }
}
