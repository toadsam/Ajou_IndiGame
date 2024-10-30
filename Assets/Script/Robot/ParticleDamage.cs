using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public int damageAmount = 10; // ��ƼŬ ������

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Monster"))
        {
            MonsterStats monsterStats = other.GetComponent<MonsterStats>();
            if (monsterStats != null)
            {
                monsterStats.TakeDamage(damageAmount);
                Debug.Log($"{other.name} took {damageAmount} damage.");
            }
        }
    }
}
