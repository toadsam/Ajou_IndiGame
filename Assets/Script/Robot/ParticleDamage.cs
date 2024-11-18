using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public int damageAmount = 10; // 파티클 데미지

    private void Start()
    {
        // 초기 설정 확인
        Debug.Log("ParticleDamage 스크립트가 시작되었습니다.");
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log($"[OnParticleCollision] 충돌한 오브젝트: {other.name}");

        if (other.CompareTag("Monster"))
        {
            MonsterStats monsterStats = other.GetComponent<MonsterStats>();
            if (monsterStats != null)
            {
                monsterStats.TakeDamage(damageAmount);
                Debug.Log($"[OnParticleCollision] {other.name}에게 {damageAmount} 데미지.");
            }
            else
            {
                Debug.LogWarning($"{other.name}에 MonsterStats 컴포넌트가 없음.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[OnTriggerEnter] 트리거 발생 - 충돌한 오브젝트: {other.name}");

        if (other.CompareTag("Monster"))
        {
            MonsterStats monsterStats = other.GetComponent<MonsterStats>();
            if (monsterStats != null)
            {
                monsterStats.TakeDamage(damageAmount);
                Debug.Log($"[OnTriggerEnter] {other.name}에게 {damageAmount} 데미지.");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[OnCollisionEnter] 충돌 발생 - 충돌한 오브젝트: {collision.gameObject.name}");

        if (collision.gameObject.CompareTag("Monster"))
        {
            MonsterStats monsterStats = collision.gameObject.GetComponent<MonsterStats>();
            if (monsterStats != null)
            {
                monsterStats.TakeDamage(damageAmount);
                Debug.Log($"[OnCollisionEnter] {collision.gameObject.name}에게 {damageAmount} 데미지.");
            }
        }
    }
}
