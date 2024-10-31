using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public int damageAmount = 10; // ��ƼŬ ������

    private void Start()
    {
        // �ʱ� ���� Ȯ��
        Debug.Log("ParticleDamage ��ũ��Ʈ�� ���۵Ǿ����ϴ�.");
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log($"[OnParticleCollision] �浹�� ������Ʈ: {other.name}");

        if (other.CompareTag("Monster"))
        {
            MonsterStats monsterStats = other.GetComponent<MonsterStats>();
            if (monsterStats != null)
            {
                monsterStats.TakeDamage(damageAmount);
                Debug.Log($"[OnParticleCollision] {other.name}���� {damageAmount} ������.");
            }
            else
            {
                Debug.LogWarning($"{other.name}�� MonsterStats ������Ʈ�� ����.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[OnTriggerEnter] Ʈ���� �߻� - �浹�� ������Ʈ: {other.name}");

        if (other.CompareTag("Monster"))
        {
            MonsterStats monsterStats = other.GetComponent<MonsterStats>();
            if (monsterStats != null)
            {
                monsterStats.TakeDamage(damageAmount);
                Debug.Log($"[OnTriggerEnter] {other.name}���� {damageAmount} ������.");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[OnCollisionEnter] �浹 �߻� - �浹�� ������Ʈ: {collision.gameObject.name}");

        if (collision.gameObject.CompareTag("Monster"))
        {
            MonsterStats monsterStats = collision.gameObject.GetComponent<MonsterStats>();
            if (monsterStats != null)
            {
                monsterStats.TakeDamage(damageAmount);
                Debug.Log($"[OnCollisionEnter] {collision.gameObject.name}���� {damageAmount} ������.");
            }
        }
    }
}
