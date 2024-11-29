using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameSkillManager : MonoBehaviour
{
    public static InGameSkillManager instance;

    public List<InGameSkill> availableSkills = new List<InGameSkill>(); // 전체 스킬 목록
    public List<InGameSkill> acquiredSkills = new List<InGameSkill>();  // 획득한 스킬 목록
    public GameObject skillSelectionUI;                                 // 스킬 선택 UI 패널
    public Button[] skillButtons;                                       // 스킬 선택 버튼 배열 (3개)

    public GameObject InGameSkillUI;

    public TextMeshProUGUI playerLevel;

    public static bool isSkills;

    private InGameSkill[] randomSkills = new InGameSkill[3];

    [Header("Skill Slots")]
    public List<GameObject> skillSlots; // 이미 생성된 슬롯들을 참조
    public int maxSkills = 4;           // 최대 스킬 개수
 // private Dictionary<string, GameObject> skillSlots = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        isSkills = false;
    }

    private void Update()
    {
        // UI가 활성화되어 있으면 마우스 커서를 표시
        if (skillSelectionUI != null && skillSelectionUI.activeSelf || (SpecialQuest.isSpecial))
        {
            Cursor.lockState = CursorLockMode.None; // 마우스 잠금 해제
            Cursor.visible = true;                 // 마우스 커서 표시
            InGameSkillUI.SetActive(false);
            Time.timeScale = 0;  // 게임 일시정지
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
            Cursor.visible = false;                  // 마우스 커서 숨기기
            InGameSkillUI.SetActive(true);
            Time.timeScale = 1;  // 게임 시작
        }
    }

    public void LevelUp()
    {
        DisplaySkillSelection();
    }

    private void DisplaySkillSelection()
    {
        isSkills = true;
        skillSelectionUI.SetActive(true);
        playerLevel.text = PlayerStats.instance.level.ToString();

        // 마우스 잠금 해제
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 중복 방지를 위해 임시 리스트 생성
        List<InGameSkill> tempAvailableSkills = new List<InGameSkill>(availableSkills);

        for (int i = 0; i < 3; i++)
        {
            // 중복 방지를 위해 랜덤 스킬 선택 후 제거
            if (tempAvailableSkills.Count == 0)
            {
                Debug.LogError("AvailableSkills에 스킬이 부족합니다.");
                break;
            }

            int randomIndex = Random.Range(0, tempAvailableSkills.Count);
            randomSkills[i] = tempAvailableSkills[randomIndex];
            tempAvailableSkills.RemoveAt(randomIndex);

            // 버튼 텍스트와 이미지 설정
            TextMeshProUGUI buttonText = skillButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            Image buttonImage = skillButtons[i].GetComponent<Image>();

            if (buttonText != null)
                buttonText.text = randomSkills[i].skillName;

            if (buttonImage != null)
                buttonImage.sprite = randomSkills[i].skillIcon;

            // 버튼 클릭 이벤트 등록
            skillButtons[i].onClick.RemoveAllListeners();
            int index = i; // 람다 캡처 문제 해결을 위해 임시 변수 사용
            skillButtons[i].onClick.AddListener(() => SelectSkill(index));
        }
    }

    private void SelectSkill(int index)
    {
        InGameSkill selectedSkill = randomSkills[index];

        // 스킬이 이미 획득되었는지 확인하고 강화 또는 획득
        if (acquiredSkills.Contains(selectedSkill))
        {
            selectedSkill.Upgrade();
            UpdateSkillDisplay();
        }
        else
        {
            // 최대 스킬 제한 확인
            if (acquiredSkills.Count < maxSkills)
            {
                acquiredSkills.Add(selectedSkill);
                Debug.Log($"{selectedSkill.skillName} 스킬을 획득했습니다!");
                selectedSkill.ActivateEffectStage(1);

                // 스킬 디스플레이 업데이트
                UpdateSkillDisplay();
            }
            else
            {
                Debug.LogWarning("스킬의 최대 개수에 도달했습니다!");
            }
        }

        skillSelectionUI.SetActive(false);

        // 마우스 잠금 복원
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UpdateSkillDisplay()
    {
        for (int i = 0; i < skillSlots.Count; i++)
        {
            if (i < acquiredSkills.Count)
            {
                InGameSkill skill = acquiredSkills[i];

                // 슬롯의 이미지와 텍스트를 설정
                Image skillImage = skillSlots[i].transform.Find("BgMask/SkillImage").GetComponent<Image>();
                TextMeshProUGUI skillLevelText = skillSlots[i].transform.Find("Label/SkillLevelText").GetComponent<TextMeshProUGUI>();

                if (skillImage != null)
                    skillImage.sprite = skill.skillIcon;

                if (skillLevelText != null)
                {
                    Debug.Log(skill.GetSkillLevel());
                    skillLevelText.text = $"Lv {skill.GetSkillLevel()}"; }
                else
                {
                    Debug.Log("없다고????");

                }

                // 슬롯 활성화
                skillSlots[i].SetActive(true);
            }
            else
            {
                // 슬롯 비활성화 (획득하지 않은 슬롯)
                skillSlots[i].SetActive(false);
            }
        }
    }
}
