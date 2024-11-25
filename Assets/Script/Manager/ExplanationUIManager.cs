using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUIManager : MonoBehaviour
{
    public GameObject tutorialUIPanel;              // Ʃ�丮�� UI ��ü �г�
    public TextMeshProUGUI tutorialText;            // ���� �ؽ�Ʈ
    public Button nextButton;                       // ���� ��ư
    public Button prevButton;                       // ���� ��ư
    public Button confirmButton;                    // Ȯ�� ��ư

    private int currentPage = 0;                    // ���� ������ �ε���
    private string[] tutorialPages =                // Ʃ�丮�� ������ ���� �迭
    {
        "Page 1: Welcome to the tutorial!",
        "Page 2: Learn how to navigate and interact.",
        "Page 3: Master advanced skills to progress."
    };

    private void Start()
    {
        tutorialUIPanel.SetActive(true);

        // ��ư Ŭ�� �̺�Ʈ ����
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PreviousPage);
        confirmButton.onClick.AddListener(CloseTutorial);

        // ù ������ UI ����
        UpdateTutorialUI();
    }

    private void Update()
    {
        // Ʃ�丮�� �г��� Ȱ��ȭ�� ���ȿ��� �׻� Ŀ���� ���̵��� ����
        if (tutorialUIPanel.activeSelf || StoreManager.isActicve)
        {
            Cursor.lockState = CursorLockMode.None;   // ���콺 ��� ����
            Cursor.visible = true;                    // ���콺 Ŀ�� ���̱�
        }
    }

    private void UpdateTutorialUI()
    {
        // ���� �������� �°� �ؽ�Ʈ�� ��ư�� ������Ʈ
        tutorialText.text = tutorialPages[currentPage];
        prevButton.gameObject.SetActive(currentPage > 0);                       // ù ������������ ���� ��ư ��Ȱ��ȭ
        nextButton.gameObject.SetActive(currentPage < tutorialPages.Length - 1); // ������ ������������ ���� ��ư ��Ȱ��ȭ
        confirmButton.gameObject.SetActive(currentPage == tutorialPages.Length - 1); // ������ ������������ Ȯ�� ��ư Ȱ��ȭ
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
        tutorialUIPanel.SetActive(false);                // Ʃ�丮�� �г� ��Ȱ��ȭ
        Cursor.lockState = CursorLockMode.Locked;        // ���콺 ��� ����
        Cursor.visible = false;                          // ���콺 Ŀ�� ��Ȱ��ȭ
    }
}
