using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager instance;

    [Header("UI Components")]
    public GameObject characterSelectUI;      // 캐릭터 선택 UI
    public Transform characterListParent;     // 캐릭터 목록 부모
    public GameObject characterSlotPrefab;    // 캐릭터 슬롯 프리팹

    public static bool isCharacterSelectUI;

    [Header("Character Detail UI")]
    public GameObject characterDetailPanel;       // 캐릭터 상세 정보 패널
    public Image detailCharacterImage;            // 상세 정보: 캐릭터 이미지
    public TextMeshProUGUI detailCharacterName;   // 상세 정보: 캐릭터 이름
    public TextMeshProUGUI detailCharacterDescription; // 상세 정보: 캐릭터 설명
    public Button selectButton;                   // 선택 버튼
    public Button cancelButton;                   // 취소 버튼

    [Header("Character Data")]
    public List<SupportCharacter> supportCharacters; // 지원 가능한 캐릭터 목록

    private SupportCharacter selectedCharacter;  // 현재 선택된 캐릭터
    private SupportCharacter activeCharacter;    // 현재 선택된 지원 캐릭터

    [Header("Activation Settings")]
    public Transform charactersParent; // 지원 캐릭터 오브젝트의 부모

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        InitializeCharacterList();

        // 초기 활성 캐릭터 설정 (없다면 null)
        activeCharacter = supportCharacters.Count > 0 ? supportCharacters[0] : null;

        if (activeCharacter != null)
        {
            Debug.Log($"{activeCharacter.characterName} 캐릭터가 기본 활성화 상태로 설정되었습니다.");
            SelectCharacter(); // 초기 활성화 처리
        }

        characterDetailPanel.SetActive(false); // 상세 정보 패널 비활성화
        isCharacterSelectUI = false;
    }

    private void Update()
    {
        // Store UI가 활성화된 동안 마우스 커서를 활성화
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
            // 슬롯 생성
            GameObject slot = Instantiate(characterSlotPrefab, characterListParent);

            // 버튼의 Image 컴포넌트 가져오기
            var slotButtonImage = slot.GetComponent<Image>();
            var slotName = slot.transform.Find("CharacterNameText").GetComponent<TextMeshProUGUI>();
            var slotSelectionText = slot.transform.Find("SelectionText").GetComponent<TextMeshProUGUI>();

            // 이미지와 이름 설정
            slotButtonImage.sprite = character.characterImage;
            slotName.text = character.characterName;

            // "선택 중" 표시 초기화
            slotSelectionText.gameObject.SetActive(character == activeCharacter);

            // 클릭 이벤트 설정
            slot.GetComponent<Button>().onClick.AddListener(() => ShowCharacterDetail(character));
        }
    }

    private void ShowCharacterDetail(SupportCharacter character)
    {
        selectedCharacter = character;

        // 상세 정보 패널 업데이트
        detailCharacterImage.sprite = character.characterImage;
        detailCharacterName.text = character.characterName;
        detailCharacterDescription.text = character.characterDescription;

        // 버튼 동작 설정
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => SelectCharacter());

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => characterDetailPanel.SetActive(false));

        characterDetailPanel.SetActive(true); // 상세 정보 패널 활성화
    }

    private void SelectCharacter()
    {
        if (charactersParent == null)
        {
            Debug.LogError("Characters Parent가 설정되지 않았습니다!");
            return;
        }

        // 모든 자식 비활성화
        foreach (Transform child in charactersParent)
        {
            child.gameObject.SetActive(false);
        }

        if (activeCharacter == null)
        {
            Debug.Log("ActiveCharacter가 설정되지 않았습니다. 초기 상태로 처리합니다.");
            return; // 선택된 캐릭터가 없으면 아무것도 활성화하지 않고 종료
        }

        // 선택된 캐릭터 이름과 동일한 자식 활성화
        Transform selectedChild = charactersParent.Find(activeCharacter.characterName);
        if (selectedChild != null)
        {
            selectedChild.gameObject.SetActive(true);
            Debug.Log($"{activeCharacter.characterName} 캐릭터가 활성화되었습니다.");
        }
        else
        {
            Debug.LogError($"'{activeCharacter.characterName}' 캐릭터를 Characters Parent에서 찾을 수 없습니다.");
        }

        // 상세 정보 패널 닫기
        characterDetailPanel.SetActive(false);
    }


}
