/*using UnityEngine;

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
    public void ActivateEffectStage(int level)
    {
        switch (level)
        {
            case 1:
                skillObject.SetActive(true);
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
}*/

using UnityEngine;

public class InGameSkill : MonoBehaviour
{
    public enum SkillType
    {
        StageEffect, // ���� �ܰ躰 ȿ�� Ȱ��ȭ
        SpawnEffect  // �÷��̾� �ֺ��� ������Ʈ ����
    }

    [Header("Skill Information")]
    public string skillName;         // ��ų �̸�
    public string skillDescription;  // ��ų ����
    public Sprite skillIcon;         // ��ų ������ �̹���
    public SkillType skillType;      // ��ų ����

    [Header("Stage Effect Settings (For StageEffect Type)")]
    public GameObject skillObject;   // ��ų ������Ʈ
    public GameObject stage1Effect;
    public GameObject stage2Effect;
    public GameObject stage3Effect;
    public GameObject stage4Effect;

    [Header("Spawn Effect Settings (For SpawnEffect Type)")]
    public GameObject spawnObject;   // ������ ������Ʈ
    public float spawnInterval = 10f; // �⺻ ���� �ֱ�
    private float currentInterval;   // ���� ���� �ֱ�
    private bool isSpawning = false; // ���� Ȱ��ȭ ����
    private float[] spawnIntervals = { 10f, 7f, 5f, 1f }; // �ܰ躰 ���� �ֱ�

    private int currentLevel = 0;
    private const int maxLevel = 4;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� ����
        ResetSkill();
    }

    // ��ų�� ��ȭ�ϴ� �޼���
    public void Upgrade()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            if (skillType == SkillType.StageEffect)
            {
                ActivateEffectStage(currentLevel);
            }
            else if (skillType == SkillType.SpawnEffect)
            {
                UpgradeSpawnEffect();
            }
        }
        else
        {
            Debug.Log($"{skillName} �ִ� ��ȭ �ܰ迡 �����߽��ϴ�.");
        }
    }

    // �ܰ躰 ȿ�� Ȱ��ȭ
    public void ActivateEffectStage(int level)
    {
        switch (level)
        {
            case 1:
                skillObject.SetActive(true);
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

    // ���� ȿ�� ��ȭ
    private void UpgradeSpawnEffect()
    {
        if (currentLevel < spawnIntervals.Length)
        {
            currentInterval = spawnIntervals[currentLevel]; // ���� ������ ���� �ֱ� ����

            if (!isSpawning)
            {
                isSpawning = true;
                InvokeRepeating(nameof(SpawnObjectAroundPlayer), 0, currentInterval);
            }
            else
            {
                CancelInvoke(nameof(SpawnObjectAroundPlayer)); // ���� ���� �ݺ� ���
                InvokeRepeating(nameof(SpawnObjectAroundPlayer), 0, currentInterval); // ���ο� �ֱ�� �缳��
            }

            Debug.Log($"{skillName} ���� ȿ���� ��ȭ�Ǿ����ϴ�. ���� �ֱ�: {currentInterval}��");
        }
        else
        {
            Debug.LogWarning($"{skillName}: �ִ� ������ �����߽��ϴ�. �� �̻� ��ȭ�� �� �����ϴ�.");
        }
    }

    // �÷��̾� �ֺ� ���� ��ġ�� ������Ʈ ����
    private void SpawnObjectAroundPlayer()
    {
        Vector3 randomPosition = playerTransform.position + Random.insideUnitSphere * 5f;
        randomPosition.y = playerTransform.position.y; // ���� ����
        Instantiate(spawnObject, randomPosition, Quaternion.identity);
        Debug.Log($"{skillName}: �÷��̾� �ֺ��� ������Ʈ ����");
    }

    // �ʱ�ȭ �޼���
    public void ResetSkill()
    {
        currentLevel = 0;
        isSpawning = false;
        CancelInvoke(nameof(SpawnObjectAroundPlayer));
        if (skillType == SkillType.StageEffect)
        {
            stage1Effect?.SetActive(false);
            stage2Effect?.SetActive(false);
            stage3Effect?.SetActive(false);
            stage4Effect?.SetActive(false);
        }
        Debug.Log($"{skillName} �ʱ�ȭ�Ǿ����ϴ�.");
    }
}

