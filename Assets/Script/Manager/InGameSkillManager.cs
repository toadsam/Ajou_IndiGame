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

    public TextMeshProUGUI playerLevel;

    public static bool isSkills;

    private InGameSkill[] randomSkills = new InGameSkill[3];

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
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
            Cursor.visible = false;                  // 마우스 커서 숨기기
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

        // 랜덤으로 3개의 스킬 선택
        for (int i = 0; i < 3; i++)
        {
            randomSkills[i] = availableSkills[Random.Range(0, availableSkills.Count)];
            TextMeshProUGUI buttonText = skillButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
                buttonText.text = randomSkills[i].skillName;

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
            selectedSkill.Upgrade();
        else
        {
            acquiredSkills.Add(selectedSkill);
            Debug.Log($"{selectedSkill.skillName} 스킬을 획득했습니다!");
            selectedSkill.ActivateEffectStage(1);
        }

        skillSelectionUI.SetActive(false);

        // 마우스 잠금 복원
      ///  Cursor.lockState = CursorLockMode.Locked;
      //  Cursor.visible = false;
    }
}
