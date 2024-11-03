using UnityEngine;

public class InGameSkill : MonoBehaviour
{
    [Header("Skill Information")]
    public string skillName;         // ��ų �̸�
    public string skillDescription;   // ��ų ����
    public Sprite skillIcon;          // ��ų ������ �̹���

    [Header("Skill Object and Effects")]
    public GameObject skillObject;    // ��ų ������Ʈ
    public GameObject stage1Effect;   // 1�ܰ� ȿ��
    public GameObject stage2Effect;   // 2�ܰ� ȿ��
    public GameObject stage3Effect;   // 3�ܰ� ȿ��
    public GameObject stage4Effect;   // 4�ܰ� ȿ��

    private int currentLevel = 0;
    private const int maxLevel = 4;

    private void Start()
    {
        ResetSkill(); // ������ �� ��� ȿ�� ��Ȱ��ȭ
    }

    // ��ų�� ��ȭ�ϴ� �޼���
    public void Upgrade()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            ActivateEffectStage(currentLevel);
        }
        else
        {
            Debug.Log("�ִ� ��ȭ �ܰ迡 �����߽��ϴ�.");
        }
    }

    // �ش� �ܰ��� ȿ�� ������Ʈ�� Ȱ��ȭ�ϴ� �޼���
    private void ActivateEffectStage(int level)
    {
        switch (level)
        {
            case 1:
                stage1Effect?.SetActive(true);
                Debug.Log($"{skillName}�� 1�ܰ� ȿ���� Ȱ��ȭ�Ǿ����ϴ�.");
                break;
            case 2:
                stage2Effect?.SetActive(true);
                Debug.Log($"{skillName}�� 2�ܰ� ȿ���� Ȱ��ȭ�Ǿ����ϴ�.");
                break;
            case 3:
                stage3Effect?.SetActive(true);
                Debug.Log($"{skillName}�� 3�ܰ� ȿ���� Ȱ��ȭ�Ǿ����ϴ�.");
                break;
            case 4:
                stage4Effect?.SetActive(true);
                Debug.Log($"{skillName}�� 4�ܰ� ȿ���� Ȱ��ȭ�Ǿ����ϴ�.");
                break;
            default:
                Debug.LogWarning("�ش� ������ �´� ȿ���� �������� �ʽ��ϴ�.");
                break;
        }
    }

    // �ʱ�ȭ �޼��� - ��ȭ �ܰ踦 �ʱ�ȭ�ϰ� ��� ȿ���� ��Ȱ��ȭ
    public void ResetSkill()
    {
        currentLevel = 0;
        stage1Effect?.SetActive(false);
        stage2Effect?.SetActive(false);
        stage3Effect?.SetActive(false);
        stage4Effect?.SetActive(false);
        Debug.Log($"{skillName}�� ��� ȿ���� �ʱ�ȭ�Ǿ����ϴ�.");
    }
}
