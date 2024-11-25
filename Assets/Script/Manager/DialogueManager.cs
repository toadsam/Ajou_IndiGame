using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("UI Elements")]
    public GameObject dialogueUIPanel;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public Button acceptButton;
    public Button declineButton;
    public Button confirmButton;

    private string[] startGameDialogue = {
        "아주 대탐험에 오신 것을 환영합니다!!",
        "재훈재훈과 함께 모험을 떠나볼까요??",
        "우선 원천관으로~~!!"
    };

    private Dictionary<string, string[]> portalDialogues = new Dictionary<string, string[]>();
    private string targetSceneName;
    private int currentDialogueIndex;
    private bool isPortalDialogue;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        InitializePortalDialogues();
        dialogueUIPanel.SetActive(false);

        nextButton.onClick.AddListener(ShowNextDialogue);
        confirmButton.onClick.AddListener(CloseStartDialogue);
        acceptButton.onClick.AddListener(OnAccept);
        declineButton.onClick.AddListener(OnDecline);

        ShowStartGameDialogue();
    }

    private void Update()
    {
        if (dialogueUIPanel.activeSelf || StoreManager.isActicve || InventoryManager.isInventoryActive)
        {
            // 마우스가 UI와 상호작용할 수 있도록 잠금 해제
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;  // 게임 일시정지
        }
        else
        {
            // UI가 닫히면 다시 잠금 설정
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;  // 게임 재개
        }
    }

    private void InitializePortalDialogues()
    {
        portalDialogues["SeonghoHallPortal"] = new string[] {
            "This portal leads to Seongho Hall.",
            "It's known for its beautiful architecture.",
            "Would you like to proceed?"
        };
        portalDialogues["WoncheonHallPortal"] = new string[] {
            "이곳은 많은 학생들이 실험으로 고통받는 곳 입니다.....",
            "이곳에 가면 실험으로 인해 변질된 몬스터,,,,마지막에는 실험으로 인해 타락한 대학생을 볼 수 있어요....",
            "들어가서 구할 준비가 되었을까요???"
        };
    }

    public void ShowStartGameDialogue()
    {
        currentDialogueIndex = 0;
        isPortalDialogue = false;
        dialogueUIPanel.SetActive(true);
        nextButton.gameObject.SetActive(true);
        confirmButton.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(false);
        declineButton.gameObject.SetActive(false);
        ShowNextDialogue();
    }

    public void ShowPortalDialogue(string portalName)
    {
        if (portalDialogues.ContainsKey(portalName))
        {
            currentDialogueIndex = 0;
            targetSceneName = portalName;
            isPortalDialogue = true;
            dialogueUIPanel.SetActive(true);
            nextButton.gameObject.SetActive(true);
            confirmButton.gameObject.SetActive(false);
            acceptButton.gameObject.SetActive(false);
            declineButton.gameObject.SetActive(false);
            ShowNextDialogue();
        }
        else
        {
            Debug.LogWarning("포탈에 대한 대사가 설정되지 않았습니다: " + portalName);
        }
    }

    private void ShowNextDialogue()
    {
        string[] currentDialogue = isPortalDialogue ? portalDialogues[targetSceneName] : startGameDialogue;

        if (currentDialogueIndex < currentDialogue.Length)
        {
            dialogueText.text = currentDialogue[currentDialogueIndex];
            currentDialogueIndex++;
        }
        else
        {
            nextButton.gameObject.SetActive(false);
            if (isPortalDialogue)
            {
                acceptButton.gameObject.SetActive(true);
                declineButton.gameObject.SetActive(true);
            }
            else
            {
                confirmButton.gameObject.SetActive(true);
            }
        }
    }

    private void CloseStartDialogue()
    {
        dialogueUIPanel.SetActive(false);
    }

    private void OnAccept()
    {
        dialogueUIPanel.SetActive(false);
        if (SceneLoader.instance != null && isPortalDialogue)
        {
            SceneLoader.instance.LoadSceneBasedOnPortalName(targetSceneName);
        }
        else
        {
            Debug.LogWarning("SceneLoader 인스턴스가 존재하지 않습니다.");
        }
    }
    private void OnDecline()
    {
        dialogueUIPanel.SetActive(false);
    }


}
