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

    public GameObject InGameSkillUI;

    public TextMeshProUGUI playerLevel;

    public static bool isSkills;

    private InGameSkill[] randomSkills = new InGameSkill[3];

    [Header("Skill Slots")]
    public List<GameObject> skillSlots; // �̹� ������ ���Ե��� ����
    public int maxSkills = 4;           // �ִ� ��ų ����
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
        // UI�� Ȱ��ȭ�Ǿ� ������ ���콺 Ŀ���� ǥ��
        if (skillSelectionUI != null && skillSelectionUI.activeSelf || (SpecialQuest.isSpecial))
        {
            Cursor.lockState = CursorLockMode.None; // ���콺 ��� ����
            Cursor.visible = true;                 // ���콺 Ŀ�� ǥ��
            InGameSkillUI.SetActive(false);
            Time.timeScale = 0;  // ���� �Ͻ�����
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // ���콺 ���
            Cursor.visible = false;                  // ���콺 Ŀ�� �����
            InGameSkillUI.SetActive(true);
            Time.timeScale = 1;  // ���� ����
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

        // ���콺 ��� ����
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // �ߺ� ������ ���� �ӽ� ����Ʈ ����
        List<InGameSkill> tempAvailableSkills = new List<InGameSkill>(availableSkills);

        for (int i = 0; i < 3; i++)
        {
            // �ߺ� ������ ���� ���� ��ų ���� �� ����
            if (tempAvailableSkills.Count == 0)
            {
                Debug.LogError("AvailableSkills�� ��ų�� �����մϴ�.");
                break;
            }

            int randomIndex = Random.Range(0, tempAvailableSkills.Count);
            randomSkills[i] = tempAvailableSkills[randomIndex];
            tempAvailableSkills.RemoveAt(randomIndex);

            // ��ư �ؽ�Ʈ�� �̹��� ����
            TextMeshProUGUI buttonText = skillButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            Image buttonImage = skillButtons[i].GetComponent<Image>();

            if (buttonText != null)
                buttonText.text = randomSkills[i].skillName;

            if (buttonImage != null)
                buttonImage.sprite = randomSkills[i].skillIcon;

            // ��ư Ŭ�� �̺�Ʈ ���
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
        {
            selectedSkill.Upgrade();
            UpdateSkillDisplay();
        }
        else
        {
            // �ִ� ��ų ���� Ȯ��
            if (acquiredSkills.Count < maxSkills)
            {
                acquiredSkills.Add(selectedSkill);
                Debug.Log($"{selectedSkill.skillName} ��ų�� ȹ���߽��ϴ�!");
                selectedSkill.ActivateEffectStage(1);

                // ��ų ���÷��� ������Ʈ
                UpdateSkillDisplay();
            }
            else
            {
                Debug.LogWarning("��ų�� �ִ� ������ �����߽��ϴ�!");
            }
        }

        skillSelectionUI.SetActive(false);

        // ���콺 ��� ����
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

                // ������ �̹����� �ؽ�Ʈ�� ����
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
                    Debug.Log("���ٰ�????");

                }

                // ���� Ȱ��ȭ
                skillSlots[i].SetActive(true);
            }
            else
            {
                // ���� ��Ȱ��ȭ (ȹ������ ���� ����)
                skillSlots[i].SetActive(false);
            }
        }
    }
}
