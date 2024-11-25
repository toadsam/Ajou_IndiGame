using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager instance;

    [Header("UI Components")]
    public GameObject characterSelectUI;      // ĳ���� ���� UI
    public Transform characterListParent;     // ĳ���� ��� �θ�
    public GameObject characterSlotPrefab;    // ĳ���� ���� ������

    public static bool isCharacterSelectUI;

    [Header("Character Detail UI")]
    public GameObject characterDetailPanel;       // ĳ���� �� ���� �г�
    public Image detailCharacterImage;            // �� ����: ĳ���� �̹���
    public TextMeshProUGUI detailCharacterName;   // �� ����: ĳ���� �̸�
    public TextMeshProUGUI detailCharacterDescription; // �� ����: ĳ���� ����
    public Button selectButton;                   // ���� ��ư
    public Button cancelButton;                   // ��� ��ư

    [Header("Character Data")]
    public List<SupportCharacter> supportCharacters; // ���� ������ ĳ���� ���

    private SupportCharacter selectedCharacter;  // ���� ���õ� ĳ����
    private SupportCharacter activeCharacter;    // ���� ���õ� ���� ĳ����

    [Header("Activation Settings")]
    public Transform charactersParent; // ���� ĳ���� ������Ʈ�� �θ�

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        InitializeCharacterList();

        // �ʱ� Ȱ�� ĳ���� ���� (���ٸ� null)
        activeCharacter = supportCharacters.Count > 0 ? supportCharacters[0] : null;

        if (activeCharacter != null)
        {
            Debug.Log($"{activeCharacter.characterName} ĳ���Ͱ� �⺻ Ȱ��ȭ ���·� �����Ǿ����ϴ�.");
            SelectCharacter(); // �ʱ� Ȱ��ȭ ó��
        }

        characterDetailPanel.SetActive(false); // �� ���� �г� ��Ȱ��ȭ
        isCharacterSelectUI = false;
    }

    private void Update()
    {
        // Store UI�� Ȱ��ȭ�� ���� ���콺 Ŀ���� Ȱ��ȭ
        if (characterSelectUI != null && characterSelectUI.activeSelf)
        {
            isCharacterSelectUI = true;
        }
        else
        {
            isCharacterSelectUI = false;
        }
    }

    private void InitializeCharacterList()
    {
        foreach (var character in supportCharacters)
        {
            // ���� ����
            GameObject slot = Instantiate(characterSlotPrefab, characterListParent);

            // ��ư�� Image ������Ʈ ��������
            var slotButtonImage = slot.GetComponent<Image>();
            var slotName = slot.transform.Find("CharacterNameText").GetComponent<TextMeshProUGUI>();
            var slotSelectionText = slot.transform.Find("SelectionText").GetComponent<TextMeshProUGUI>();

            // �̹����� �̸� ����
            slotButtonImage.sprite = character.characterImage;
            slotName.text = character.characterName;

            // "���� ��" ǥ�� �ʱ�ȭ
            slotSelectionText.gameObject.SetActive(character == activeCharacter);

            // Ŭ�� �̺�Ʈ ����
            slot.GetComponent<Button>().onClick.AddListener(() => ShowCharacterDetail(character));
        }
    }

    private void ShowCharacterDetail(SupportCharacter character)
    {
        selectedCharacter = character;

        // �� ���� �г� ������Ʈ
        detailCharacterImage.sprite = character.characterImage;
        detailCharacterName.text = character.characterName;
        detailCharacterDescription.text = character.characterDescription;

        // ��ư ���� ����
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => SelectCharacter());

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => characterDetailPanel.SetActive(false));

        characterDetailPanel.SetActive(true); // �� ���� �г� Ȱ��ȭ
    }

    private void SelectCharacter()
    {
        if (charactersParent == null)
        {
            Debug.LogError("Characters Parent�� �������� �ʾҽ��ϴ�!");
            return;
        }

        // ��� �ڽ� ��Ȱ��ȭ
        foreach (Transform child in charactersParent)
        {
            child.gameObject.SetActive(false);
        }

        if (activeCharacter == null)
        {
            Debug.Log("ActiveCharacter�� �������� �ʾҽ��ϴ�. �ʱ� ���·� ó���մϴ�.");
            return; // ���õ� ĳ���Ͱ� ������ �ƹ��͵� Ȱ��ȭ���� �ʰ� ����
        }

        // ���õ� ĳ���� �̸��� ������ �ڽ� Ȱ��ȭ
        Transform selectedChild = charactersParent.Find(activeCharacter.characterName);
        if (selectedChild != null)
        {
            selectedChild.gameObject.SetActive(true);
            Debug.Log($"{activeCharacter.characterName} ĳ���Ͱ� Ȱ��ȭ�Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError($"'{activeCharacter.characterName}' ĳ���͸� Characters Parent���� ã�� �� �����ϴ�.");
        }

        // �� ���� �г� �ݱ�
        characterDetailPanel.SetActive(false);
    }


}
