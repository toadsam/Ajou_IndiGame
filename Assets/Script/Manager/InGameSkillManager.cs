using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class InGameSkillManager : MonoBehaviour
{
    public static InGameSkillManager instance;

    public List<InGameSkill> availableSkills = new List<InGameSkill>(); // ��ü ��ų ���
    public List<InGameSkill> acquiredSkills = new List<InGameSkill>(); // ȹ���� ��ų ���
    public GameObject skillSelectionUI; // ��ų ���� UI
    public Button[] skillButtons; // UI���� ���� ������ ��ų ��ư��
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

        // �������� 3���� ��ų ����
        for (int i = 0; i < 3; i++)
        {
            randomSkills[i] = availableSkills[Random.Range(0, availableSkills.Count)];
            skillButtons[i].GetComponentInChildren<Text>().text = randomSkills[i].skillName;
            skillButtons[i].onClick.RemoveAllListeners();
            int index = i; // Ŭ���� ���� �ذ��� ���� �ε��� ���� ����
            skillButtons[i].onClick.AddListener(() => SelectSkill(index));
        }
    }

    private void SelectSkill(int index)
    {
        InGameSkill selectedSkill = randomSkills[index];

        // �̹� ������ ��ų���� Ȯ��
        if (acquiredSkills.Contains(selectedSkill))
        {
            selectedSkill.Upgrade();
        }
        else
        {
            acquiredSkills.Add(selectedSkill);
            Debug.Log($"{selectedSkill.skillName} ��ų�� ȹ���߽��ϴ�!");
        }

        skillSelectionUI.SetActive(false);
    }
}
