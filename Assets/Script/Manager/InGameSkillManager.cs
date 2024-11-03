using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class InGameSkillManager : MonoBehaviour
{
    public static InGameSkillManager instance;

    public List<InGameSkill> availableSkills = new List<InGameSkill>(); // 전체 스킬 목록
    public List<InGameSkill> acquiredSkills = new List<InGameSkill>(); // 획득한 스킬 목록
    public GameObject skillSelectionUI; // 스킬 선택 UI
    public Button[] skillButtons; // UI에서 선택 가능한 스킬 버튼들
    private InGameSkill[] randomSkills = new InGameSkill[3];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LevelUp()
    {
        DisplaySkillSelection();
    }

    private void DisplaySkillSelection()
    {
        skillSelectionUI.SetActive(true);

        // 랜덤으로 3개의 스킬 선택
        for (int i = 0; i < 3; i++)
        {
            randomSkills[i] = availableSkills[Random.Range(0, availableSkills.Count)];
            skillButtons[i].GetComponentInChildren<Text>().text = randomSkills[i].skillName;
            skillButtons[i].onClick.RemoveAllListeners();
            int index = i; // 클로저 문제 해결을 위해 인덱스 변수 생성
            skillButtons[i].onClick.AddListener(() => SelectSkill(index));
        }
    }

    private void SelectSkill(int index)
    {
        InGameSkill selectedSkill = randomSkills[index];

        // 이미 보유한 스킬인지 확인
        if (acquiredSkills.Contains(selectedSkill))
        {
            selectedSkill.Upgrade();
        }
        else
        {
            acquiredSkills.Add(selectedSkill);
            Debug.Log($"{selectedSkill.skillName} 스킬을 획득했습니다!");
        }

        skillSelectionUI.SetActive(false);
    }
}
