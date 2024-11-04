using UnityEngine;

public class BasicSkill : MonoBehaviour
{
    public int damageAmount = 10; // ������ ��
    public ParticleSystem particleSystem; // ��ƼŬ �ý��� ����

    private void Start()
    {
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>(); // ��ƼŬ �ý����� ���� ���, ���� ������Ʈ���� ã�Ƽ� �Ҵ�
        }

        if (particleSystem == null)
        {
            Debug.LogError("��ƼŬ �ý����� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Monster")) // �浹�� ��ü�� 'Monster' �±׸� �������� Ȯ��
        {
            // MonsterStats�� �����ͼ� �������� ����
            MonsterStats monsterStats = other.GetComponent<MonsterStats>();
            if (monsterStats != null)
            {
                monsterStats.TakeDamage(damageAmount);
                Debug.Log($"{other.name}���� {damageAmount}�� �������� �������ϴ�.");
            }
            else
            {
                Debug.LogWarning($"{other.name} ������Ʈ�� MonsterStats ������Ʈ�� �����ϴ�.");
            }
        }
    }
}
