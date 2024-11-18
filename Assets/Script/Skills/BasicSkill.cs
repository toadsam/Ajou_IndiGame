using UnityEngine;

public class BasicSkill : MonoBehaviour
{
    public int damageAmount = 10; // 데미지 양
    public ParticleSystem particleSystem; // 파티클 시스템 참조

    private void Start()
    {
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>(); // 파티클 시스템이 없을 경우, 현재 오브젝트에서 찾아서 할당
        }

        if (particleSystem == null)
        {
            Debug.LogError("파티클 시스템이 할당되지 않았습니다.");
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Monster")) // 충돌한 객체가 'Monster' 태그를 가졌는지 확인
        {
            // MonsterStats를 가져와서 데미지를 입힘
            MonsterStats monsterStats = other.GetComponent<MonsterStats>();
            if (monsterStats != null)
            {
                monsterStats.TakeDamage(damageAmount);
                Debug.Log($"{other.name}에게 {damageAmount}의 데미지를 입혔습니다.");
            }
            else
            {
                Debug.LogWarning($"{other.name} 오브젝트에 MonsterStats 컴포넌트가 없습니다.");
            }
        }
    }
}
