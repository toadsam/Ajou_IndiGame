using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Skill3 skill3; // Skill3 스크립트를 참조할 변수

    private void Start()
    {
        if (skill3 == null)
        {
            Debug.LogError("Skill3 스크립트가 할당되지 않았습니다. 에디터에서 설정해 주세요.");
        }
    }

    private void Update()
    {
        // E 버튼을 눌러 파티클 시스템을 켜고 끄기
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleSkill();
        }
    }

    private void ToggleSkill()
    {
        if (skill3 != null)
        {
            skill3.ToggleParticleSystem();
        }
    }
}
