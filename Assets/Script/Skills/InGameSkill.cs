/*using UnityEngine;

public class InGameSkill : MonoBehaviour
{
    [Header("Skill Information")]
    public string skillName;         // 스킬 이름
    public string skillDescription;   // 스킬 설명
    public Sprite skillIcon;          // 스킬 아이콘 이미지

    [Header("Skill Object and Effects")]
    public GameObject skillObject;    // 스킬 오브젝트
    public GameObject stage1Effect;   // 1단계 효과
    public GameObject stage2Effect;   // 2단계 효과
    public GameObject stage3Effect;   // 3단계 효과
    public GameObject stage4Effect;   // 4단계 효과

    private int currentLevel = 0;
    private const int maxLevel = 4;

    private void Start()
    {
        ResetSkill(); // 시작할 때 모든 효과 비활성화
    }

    // 스킬을 강화하는 메서드
    public void Upgrade()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            ActivateEffectStage(currentLevel);
        }
        else
        {
            Debug.Log("최대 강화 단계에 도달했습니다.");
        }
    }

    // 해당 단계의 효과 오브젝트를 활성화하는 메서드
    public void ActivateEffectStage(int level)
    {
        switch (level)
        {
            case 1:
                skillObject.SetActive(true);
                stage1Effect?.SetActive(true);
                Debug.Log($"{skillName}의 1단계 효과가 활성화되었습니다.");
                break;
            case 2:
                stage2Effect?.SetActive(true);
                Debug.Log($"{skillName}의 2단계 효과가 활성화되었습니다.");
                break;
            case 3:
                stage3Effect?.SetActive(true);
                Debug.Log($"{skillName}의 3단계 효과가 활성화되었습니다.");
                break;
            case 4:
                stage4Effect?.SetActive(true);
                Debug.Log($"{skillName}의 4단계 효과가 활성화되었습니다.");
                break;
            default:
                Debug.LogWarning("해당 레벨에 맞는 효과가 존재하지 않습니다.");
                break;
        }
    }

    // 초기화 메서드 - 강화 단계를 초기화하고 모든 효과를 비활성화
    public void ResetSkill()
    {
        currentLevel = 0;
        stage1Effect?.SetActive(false);
        stage2Effect?.SetActive(false);
        stage3Effect?.SetActive(false);
        stage4Effect?.SetActive(false);
        Debug.Log($"{skillName}의 모든 효과가 초기화되었습니다.");
    }
}*/

using UnityEngine;

public class InGameSkill : MonoBehaviour
{
    public enum SkillType
    {
        StageEffect, // 기존 단계별 효과 활성화
        SpawnEffect  // 플레이어 주변에 오브젝트 생성
    }

    [Header("Skill Information")]
    public string skillName;         // 스킬 이름
    public string skillDescription;  // 스킬 설명
    public Sprite skillIcon;         // 스킬 아이콘 이미지
    public SkillType skillType;      // 스킬 유형

    [Header("Stage Effect Settings (For StageEffect Type)")]
    public GameObject skillObject;   // 스킬 오브젝트
    public GameObject stage1Effect;
    public GameObject stage2Effect;
    public GameObject stage3Effect;
    public GameObject stage4Effect;

    [Header("Spawn Effect Settings (For SpawnEffect Type)")]
    public GameObject spawnObject;   // 생성될 오브젝트
    public float spawnInterval = 10f; // 기본 생성 주기
    private float currentInterval;   // 현재 생성 주기
    private bool isSpawning = false; // 스폰 활성화 여부
    private float[] spawnIntervals = { 10f, 7f, 5f, 1f }; // 단계별 생성 주기

    private int currentLevel = 0;
    private const int maxLevel = 4;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 참조
        ResetSkill();
    }

    // 스킬을 강화하는 메서드
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
            Debug.Log($"{skillName} 최대 강화 단계에 도달했습니다.");
        }
    }

    // 단계별 효과 활성화
    public void ActivateEffectStage(int level)
    {
        switch (level)
        {
            case 1:
                skillObject.SetActive(true);
                stage1Effect?.SetActive(true);
                Debug.Log($"{skillName}의 1단계 효과가 활성화되었습니다.");
                break;
            case 2:
                stage2Effect?.SetActive(true);
                Debug.Log($"{skillName}의 2단계 효과가 활성화되었습니다.");
                break;
            case 3:
                stage3Effect?.SetActive(true);
                Debug.Log($"{skillName}의 3단계 효과가 활성화되었습니다.");
                break;
            case 4:
                stage4Effect?.SetActive(true);
                Debug.Log($"{skillName}의 4단계 효과가 활성화되었습니다.");
                break;
            default:
                Debug.LogWarning("해당 레벨에 맞는 효과가 존재하지 않습니다.");
                break;
        }
    }

    // 스폰 효과 강화
    private void UpgradeSpawnEffect()
    {
        if (currentLevel < spawnIntervals.Length)
        {
            currentInterval = spawnIntervals[currentLevel]; // 현재 레벨에 따른 주기 설정

            if (!isSpawning)
            {
                isSpawning = true;
                InvokeRepeating(nameof(SpawnObjectAroundPlayer), 0, currentInterval);
            }
            else
            {
                CancelInvoke(nameof(SpawnObjectAroundPlayer)); // 기존 스폰 반복 취소
                InvokeRepeating(nameof(SpawnObjectAroundPlayer), 0, currentInterval); // 새로운 주기로 재설정
            }

            Debug.Log($"{skillName} 스폰 효과가 강화되었습니다. 현재 주기: {currentInterval}초");
        }
        else
        {
            Debug.LogWarning($"{skillName}: 최대 레벨에 도달했습니다. 더 이상 강화할 수 없습니다.");
        }
    }

    // 플레이어 주변 랜덤 위치에 오브젝트 생성
    private void SpawnObjectAroundPlayer()
    {
        Vector3 randomPosition = playerTransform.position + Random.insideUnitSphere * 5f;
        randomPosition.y = playerTransform.position.y; // 높이 유지
        Instantiate(spawnObject, randomPosition, Quaternion.identity);
        Debug.Log($"{skillName}: 플레이어 주변에 오브젝트 생성");
    }

    // 초기화 메서드
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
        Debug.Log($"{skillName} 초기화되었습니다.");
    }
}

