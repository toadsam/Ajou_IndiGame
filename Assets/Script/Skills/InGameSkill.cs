using UnityEngine;

public class InGameSkill : MonoBehaviour
{
    public enum SkillType
    {
        StageEffect,  // ���� �ܰ躰 ȿ�� Ȱ��ȭ
        SpawnEffect,  // �÷��̾� �ֺ��� ������Ʈ ����
        ParticleEffect // ��ƼŬ ���� �� ũ�� ����
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

    [Header("Particle Effect Settings (For ParticleEffect Type)")]
    public GameObject particlePrefab; // ������ ��ƼŬ ������
    public float[] particleSizes = { 0.5f, 1f, 1.5f, 2f }; // ������ Start Size ��
    private bool isParticleActive = false; // ��ƼŬ Ȱ��ȭ ����
    [SerializeField] private float particleInterval = 3f;

    private int currentLevel = 0;
    private const int maxLevel = 4;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerTransform == null)
        {
            Debug.LogError("Player Transform not found. Ensure your player has the 'Player' tag.");
        }

        ResetSkill();
    }

    public void Upgrade()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            switch (skillType)
            {
                case SkillType.StageEffect:
                    ActivateEffectStage(currentLevel);
                    break;

                case SkillType.SpawnEffect:
                    UpgradeSpawnEffect();
                    break;

                case SkillType.ParticleEffect:
                    UpgradeParticleEffect();
                    break;
            }
        }
        else
        {
            Debug.Log($"{skillName} �ִ� ��ȭ �ܰ迡 �����߽��ϴ�.");
        }
    }

    public void ActivateEffectStage(int level)
    {
        switch (level)
        {
            case 1:
                skillObject?.SetActive(true);
                stage1Effect?.SetActive(true);
                break;
            case 2:
                stage2Effect?.SetActive(true);
                break;
            case 3:
                stage3Effect?.SetActive(true);
                break;
            case 4:
                stage4Effect?.SetActive(true);
                break;
            default:
                Debug.LogWarning("Invalid level for Stage Effect.");
                break;
        }
    }

    private void UpgradeSpawnEffect()
    {
        if (currentLevel < spawnIntervals.Length)
        {
            currentInterval = spawnIntervals[currentLevel];

            if (!isSpawning)
            {
                isSpawning = true;
                InvokeRepeating(nameof(SpawnObjectAroundPlayer), 0, currentInterval);
            }
            else
            {
                CancelInvoke(nameof(SpawnObjectAroundPlayer));
                InvokeRepeating(nameof(SpawnObjectAroundPlayer), 0, currentInterval);
            }
        }
    }

    private void UpgradeParticleEffect()
    {
        if (currentLevel < particleSizes.Length)
        {
            if (!isParticleActive)
            {
                isParticleActive = true;
                InvokeRepeating(nameof(SpawnParticleAroundPlayer), 0, particleInterval);
            }
        }
    }

    private void SpawnObjectAroundPlayer()
    {
        if (spawnObject == null || playerTransform == null)
            return;

        Vector3 randomPosition = playerTransform.position + Random.insideUnitSphere * 5f;
        randomPosition.y = playerTransform.position.y;
        Instantiate(spawnObject, randomPosition, Quaternion.identity);
    }

    private void SpawnParticleAroundPlayer()
    {
        if (particlePrefab == null || currentLevel <= 0 || playerTransform == null)
            return;

        GameObject particle = Instantiate(particlePrefab, playerTransform.position + Random.insideUnitSphere * 3f, Quaternion.identity);
        ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            var main = particleSystem.main;
            main.startSize = particleSizes[currentLevel - 1];
        }
    }

    public void ResetSkill()
    {
        currentLevel = 0;
        isSpawning = false;
        isParticleActive = false;
        CancelInvoke(nameof(SpawnObjectAroundPlayer));
        CancelInvoke(nameof(SpawnParticleAroundPlayer));
    }
}
