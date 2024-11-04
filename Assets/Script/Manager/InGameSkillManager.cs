using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameSkillManager : MonoBehaviour
{
    public static InGameSkillManager instance;

    public List<InGameSkill> availableSkills = new List<InGameSkill>(); // ��ü ��ų ���
    public List<InGameSkill> acquiredSkills = new List<InGameSkill>();  // ȹ���� ��ų ���
    public GameObject skillSelectionUI;                                 // ��ų ���� UI �г�
    public Button[] skillButtons;                                       // ��ų ���� ��ư �迭 (3��)

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

        // ���콺 ��� ����
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // �������� 3���� ��ų ����
        for (int i = 0; i < 3; i++)
        {
            randomSkills[i] = availableSkills[Random.Range(0, availableSkills.Count)];
            TextMeshProUGUI buttonText = skillButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
                buttonText.text = randomSkills[i].skillName;

            skillButtons[i].onClick.RemoveAllListeners();
            int index = i; // ���� ĸó ���� �ذ��� ���� �ӽ� ���� ���
            skillButtons[i].onClick.AddListener(() => SelectSkill(index));
        }
    }

    private void SelectSkill(int index)
    {
        InGameSkill selectedSkill = randomSkills[index];

        // ��ų�� �̹� ȹ��Ǿ����� Ȯ���ϰ� ��ȭ �Ǵ� ȹ��
        if (acquiredSkills.Contains(selectedSkill))
            selectedSkill.Upgrade();
        else
        {
            acquiredSkills.Add(selectedSkill);
            Debug.Log($"{selectedSkill.skillName} ��ų�� ȹ���߽��ϴ�!");
            selectedSkill.ActivateEffectStage(1);
        }

        skillSelectionUI.SetActive(false);

        // ���콺 ��� ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}