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

    private InGameSkill[] randomSkills = new InGameSkill[3];

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void LevelUp()
    {
        DisplaySkillSelection();
    }

    private void DisplaySkillSelection()
    {
        skillSelectionUI.SetActive(true);

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
        }

        skillSelectionUI.SetActive(false);

        // 마우스 잠금 복원
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
