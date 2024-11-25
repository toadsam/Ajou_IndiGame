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
        "���� ��Ž�迡 ���� ���� ȯ���մϴ�!!",
        "�������ư� �Բ� ������ ���������??",
        "�켱 ��õ������~~!!"
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
            // ���콺�� UI�� ��ȣ�ۿ��� �� �ֵ��� ��� ����
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;  // ���� �Ͻ�����
        }
        else
        {
            // UI�� ������ �ٽ� ��� ����
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;  // ���� �簳
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
            "�̰��� ���� �л����� �������� ����޴� �� �Դϴ�.....",
            "�̰��� ���� �������� ���� ������ ����,,,,���������� �������� ���� Ÿ���� ���л��� �� �� �־��....",
            "���� ���� �غ� �Ǿ������???"
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
            Debug.LogWarning("��Ż�� ���� ��簡 �������� �ʾҽ��ϴ�: " + portalName);
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
            Debug.LogWarning("SceneLoader �ν��Ͻ��� �������� �ʽ��ϴ�.");
        }
    }
    private void OnDecline()
    {
        dialogueUIPanel.SetActive(false);
    }


}
