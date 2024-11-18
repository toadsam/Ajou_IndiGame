using UnityEngine;
using System.Collections.Generic;

public class Skill3 : MonoBehaviour
{
    public int damageAmount = 10; // ��ƼŬ ������
    public ParticleSystem particleSystem; // ��ƼŬ �ý��� ����

    private ParticleSystem.CollisionModule collisionModule;
    private ParticleSystem.TriggerModule triggerModule;
    private bool isParticleActive = false; // ��ƼŬ Ȱ��ȭ ����

    private void Start()
    {
        if (particleSystem == null)
        {
            Debug.LogError("Particle System�� �Ҵ���� �ʾҽ��ϴ�. �����Ϳ��� �Ҵ��� �ּ���.");
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
            Debug.Log("��ƼŬ�� Ȱ��ȭ�Ǿ����ϴ�.");
        }
        else
        {
            particleSystem.Stop();
            Debug.Log("��ƼŬ�� ��Ȱ��ȭ�Ǿ����ϴ�.");
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
                Debug.Log($"{other.name}���� {damageAmount}�� �������� �������ϴ�. OnParticleCollision �߻�");
            }
            else
            {
                Debug.LogWarning($"{other.name}�� MonsterStats�� �����ϴ�.");
            }
        }
        else
        {
            Debug.Log("OnParticleCollision�� �߻����� ����. ��ƼŬ Ȱ�� ����: " + isParticleActive);
        }
    }

   
}
