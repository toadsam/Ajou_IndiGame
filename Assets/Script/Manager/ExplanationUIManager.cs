using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUIManager : MonoBehaviour
{
    public GameObject tutorialUIPanel;              // 튜토리얼 UI 전체 패널
    public TextMeshProUGUI tutorialText;            // 설명 텍스트
    public Button nextButton;                       // 다음 버튼
    public Button prevButton;                       // 이전 버튼
    public Button confirmButton;                    // 확인 버튼

    private int currentPage = 0;                    // 현재 페이지 인덱스
    private string[] tutorialPages =                // 튜토리얼 페이지 설명 배열
    {
        "Page 1: Welcome to the tutorial!",
        "Page 2: Learn how to navigate and interact.",
        "Page 3: Master advanced skills to progress."
    };

    private void Start()
    {
        tutorialUIPanel.SetActive(true);

        // 버튼 클릭 이벤트 설정
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PreviousPage);
        confirmButton.onClick.AddListener(CloseTutorial);

        // 첫 페이지 UI 설정
        UpdateTutorialUI();
    }

    private void Update()
    {
        // 튜토리얼 패널이 활성화된 동안에는 항상 커서가 보이도록 설정
        if (tutorialUIPanel.activeSelf || StoreManager.isActicve)
        {
            Cursor.lockState = CursorLockMode.None;   // 마우스 잠금 해제
            Cursor.visible = true;                    // 마우스 커서 보이기
        }
    }

    private void UpdateTutorialUI()
    {
        // 현재 페이지에 맞게 텍스트와 버튼을 업데이트
        tutorialText.text = tutorialPages[currentPage];
        prevButton.gameObject.SetActive(currentPage > 0);                       // 첫 페이지에서는 이전 버튼 비활성화
        nextButton.gameObject.SetActive(currentPage < tutorialPages.Length - 1); // 마지막 페이지에서는 다음 버튼 비활성화
        confirmButton.gameObject.SetActive(currentPage == tutorialPages.Length - 1); // 마지막 페이지에서만 확인 버튼 활성화
    }

    private void NextPage()
    {
        if (currentPage < tutorialPages.Length - 1)
        {
            currentPage++;
            UpdateTutorialUI();
        }
    }

    private void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateTutorialUI();
        }
    }

    private void CloseTutorial()
    {
        tutorialUIPanel.SetActive(false);                // 튜토리얼 패널 비활성화
        Cursor.lockState = CursorLockMode.Locked;        // 마우스 잠금 설정
        Cursor.visible = false;                          // 마우스 커서 비활성화
    }
}
