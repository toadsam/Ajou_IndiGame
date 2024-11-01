using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Skill3 skill3; // Skill3 ��ũ��Ʈ�� ������ ����

    private void Start()
    {
        if (skill3 == null)
        {
            Debug.LogError("Skill3 ��ũ��Ʈ�� �Ҵ���� �ʾҽ��ϴ�. �����Ϳ��� ������ �ּ���.");
        }
    }

    private void Update()
    {
        // E ��ư�� ���� ��ƼŬ �ý����� �Ѱ� ����
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
