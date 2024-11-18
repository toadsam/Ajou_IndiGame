using UnityEngine;
using System.Collections.Generic;

public class Skill2 : MonoBehaviour
{
    public int damageAmount = 10; // ��ƼŬ ������
    public ParticleSystem particleSystem; // ��ƼŬ �ý��� ����
    public GameObject Player; // �÷��̾� ��ü
    public GameObject PlayerBody;
    public GameObject PlayerWeapon;

    private bool isParticleActive = false; // ��ƼŬ Ȱ��ȭ ����
    private List<Collider> monsterColliders = new List<Collider>(); // Monster �±��� Collider ����Ʈ
    private Collider playerCollider; // Player�� Collider ����
    private Rigidbody playerRigidbody; // Player�� Rigidbody ����

    private void Start()
    {
        if (particleSystem == null)
        {
            Debug.LogError("Particle System�� �Ҵ���� �ʾҽ��ϴ�. �����Ϳ��� �Ҵ��� �ּ���.");
            return;
        }

        if (Player != null)
        {
            playerCollider = Player.GetComponent<Collider>();
            playerRigidbody = Player.GetComponent<Rigidbody>();

            if (playerRigidbody == null)
            {
                Debug.LogError("Player ��ü�� Rigidbody�� �����ϴ�.");
                return;
            }
        }
        else
        {
            Debug.LogError("Player ��ü�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        // Monster �±��� ��� ��ü ã��
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (var monster in monsters)
        {
            Collider monsterCollider = monster.GetComponent<Collider>();
            if (monsterCollider != null)
            {
                monsterColliders.Add(monsterCollider);
            }
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
            PlayerBody.SetActive(false);
            PlayerWeapon.SetActive(false);

            // Monster �±��� Collider �� Player Collider���� �浹 ���� ����
            SetCollisionWithMonsters(false);

            // Player�� Collider�� Ʈ���ŷ� �����ϰ� �߷��� ��Ȱ��ȭ
            playerCollider.isTrigger = true;
            playerRigidbody.useGravity = false;
        }
        else
        {
            particleSystem.Stop();
            Debug.Log("��ƼŬ�� ��Ȱ��ȭ�Ǿ����ϴ�.");
            PlayerBody.SetActive(true);
            PlayerWeapon.SetActive(true);

            // Monster �±��� Collider �� Player Collider���� �浹 �ٽ� Ȱ��ȭ
            SetCollisionWithMonsters(true);

            // Player�� Collider�� �Ϲ� Collider�� �ǵ����� �߷��� Ȱ��ȭ
            playerCollider.isTrigger = false;
            playerRigidbody.useGravity = true;
        }
    }

    // Monster �±��� Collider���� �浹 ���� ���� �޼���
    private void SetCollisionWithMonsters(bool enableCollision)
    {
        Collider skillCollider = GetComponent<Collider>(); // ���� ��ü�� Collider
        if (skillCollider == null) return;

        // Monster���� �浹 ����
        foreach (Collider monsterCollider in monsterColliders)
        {
            if (monsterCollider != null)
            {
                Physics.IgnoreCollision(skillCollider, monsterCollider, !enableCollision);
                if (playerCollider != null)
                {
                    Physics.IgnoreCollision(playerCollider, monsterCollider, !enableCollision);
                }
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (isParticleActive && other.CompareTag("Monster"))
        {
            MonsterStats monsterStats = other.GetComponent<MonsterStats>();
            Rigidbody monsterRigidbody = other.GetComponent<Rigidbody>();

            if (monsterStats != null)
            {
                monsterStats.TakeDamage(damageAmount);
                Debug.Log($"{other.name}���� {damageAmount}�� �������� �������ϴ�. OnParticleCollision �߻�");
            }
            else
            {
                Debug.LogWarning($"{other.name}�� MonsterStats�� �����ϴ�.");
            }

            // �ε巴�� �о��
            if (monsterRigidbody != null)
            {
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                monsterRigidbody.AddForce(knockbackDirection * 200f, ForceMode.VelocityChange); // �ε巴�� �о�� ��
            }
        }
        else
        {
            Debug.Log("OnParticleCollision�� �߻����� ����. ��ƼŬ Ȱ�� ����: " + isParticleActive);
        }
    }
}
