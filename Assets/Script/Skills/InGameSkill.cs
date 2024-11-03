using UnityEngine;

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
    private void ActivateEffectStage(int level)
    {
        switch (level)
        {
            case 1:
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
}
