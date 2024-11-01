using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Skill2 skill2; // Skill2 스크립트를 참조할 변수
    public Skill3 skill3; // Skill3 스크립트를 참조할 변수

    private void Start()
    {
        if (skill2 == null)
        {
            Debug.LogError("Skill2 스크립트가 할당되지 않았습니다. 에디터에서 설정해 주세요.");
        }
        if (skill3 == null)
        {
            Debug.LogError("Skill3 스크립트가 할당되지 않았습니다. 에디터에서 설정해 주세요.");
        }
    }

    private void Update()
    {
        // R 버튼을 눌러 Skill2 실행
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleSkill2();
        }
        // E 버튼을 눌러 Skill3 실행
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleSkill3();
        }
    }

    private void ToggleSkill2()
    {
        if (skill2 != null)
        {
            skill2.ToggleParticleSystem();
        }
    }

    private void ToggleSkill3()
    {
        if (skill3 != null)
        {
            skill3.ToggleParticleSystem();
        }
    }
}
