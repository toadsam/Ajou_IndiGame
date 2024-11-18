using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public string sceneToLoad; // 이동할 씬 이름을 에디터에서 설정할 수 있도록 변수 선언
    public Button startButton; // 버튼을 에디터에서 할당

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
    }

    private void OnStartButtonClicked()
    {
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
