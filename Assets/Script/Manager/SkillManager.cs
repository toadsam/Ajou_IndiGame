using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Skill2 skill2; // Skill2 ��ũ��Ʈ�� ������ ����
    public Skill3 skill3; // Skill3 ��ũ��Ʈ�� ������ ����

    private void Start()
    {
        if (skill2 == null)
        {
            Debug.LogError("Skill2 ��ũ��Ʈ�� �Ҵ���� �ʾҽ��ϴ�. �����Ϳ��� ������ �ּ���.");
        }
        if (skill3 == null)
        {
            Debug.LogError("Skill3 ��ũ��Ʈ�� �Ҵ���� �ʾҽ��ϴ�. �����Ϳ��� ������ �ּ���.");
        }
    }

    private void Update()
    {
        // R ��ư�� ���� Skill2 ����
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleSkill2();
        }
        // E ��ư�� ���� Skill3 ����
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
