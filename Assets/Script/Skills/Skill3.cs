using UnityEngine;
using System.Collections.Generic;

public class Skill3 : MonoBehaviour
{
    public int damageAmount = 10; // 파티클 데미지
    public ParticleSystem particleSystem; // 파티클 시스템 참조

    private ParticleSystem.CollisionModule collisionModule;
    private ParticleSystem.TriggerModule triggerModule;
    private bool isParticleActive = false; // 파티클 활성화 상태

    private void Start()
    {
        if (particleSystem == null)
        {
            Debug.LogError("Particle System이 할당되지 않았습니다. 에디터에서 할당해 주세요.");
            return;
        }



        particleSystem.Stop();
    }

    public void ToggleParticleSystem()
    {
        isParticleActive = !isParticleActive;

        if (isParticleActive)
        {
            particleSystem.Play();
            Debug.Log("파티클이 활성화되었습니다.");
        }
        else
        {
            particleSystem.Stop();
            Debug.Log("파티클이 비활성화되었습니다.");
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (isParticleActive && other.CompareTag("Monster"))
        {
            MonsterStats monsterStats = other.GetComponent<MonsterStats>();
            if (monsterStats != null)
            {
                monsterStats.TakeDamage(damageAmount);
                Debug.Log($"{other.name}에게 {damageAmount}의 데미지를 입혔습니다. OnParticleCollision 발생");
            }
            else
            {
                Debug.LogWarning($"{other.name}에 MonsterStats가 없습니다.");
            }
        }
        else
        {
            Debug.Log("OnParticleCollision이 발생하지 않음. 파티클 활성 상태: " + isParticleActive);
        }
    }

   
}
