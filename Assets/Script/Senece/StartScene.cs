using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public string sceneToLoad; // �̵��� �� �̸��� �����Ϳ��� ������ �� �ֵ��� ���� ����
    public Button startButton; // ��ư�� �����Ϳ��� �Ҵ�

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
    }

    private void OnStartButtonClicked()
    {
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
