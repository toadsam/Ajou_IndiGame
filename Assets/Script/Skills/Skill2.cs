using UnityEngine;
using System.Collections.Generic;

public class Skill2 : MonoBehaviour
{
    public int damageAmount = 10; // 파티클 데미지
    public ParticleSystem particleSystem; // 파티클 시스템 참조
    public GameObject Player; // 플레이어 객체
    public GameObject PlayerBody;
    public GameObject PlayerWeapon;

    private bool isParticleActive = false; // 파티클 활성화 상태
    private List<Collider> monsterColliders = new List<Collider>(); // Monster 태그의 Collider 리스트
    private Collider playerCollider; // Player의 Collider 참조
    private Rigidbody playerRigidbody; // Player의 Rigidbody 참조

    private void Start()
    {
        if (particleSystem == null)
        {
            Debug.LogError("Particle System이 할당되지 않았습니다. 에디터에서 할당해 주세요.");
            return;
        }

        if (Player != null)
        {
            playerCollider = Player.GetComponent<Collider>();
            playerRigidbody = Player.GetComponent<Rigidbody>();

            if (playerRigidbody == null)
            {
                Debug.LogError("Player 객체에 Rigidbody가 없습니다.");
                return;
            }
        }
        else
        {
            Debug.LogError("Player 객체가 할당되지 않았습니다.");
            return;
        }

        // Monster 태그의 모든 객체 찾기
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
            Debug.Log("파티클이 활성화되었습니다.");
            PlayerBody.SetActive(false);
            PlayerWeapon.SetActive(false);

            // Monster 태그의 Collider 및 Player Collider와의 충돌 무시 설정
            SetCollisionWithMonsters(false);

            // Player의 Collider를 트리거로 설정하고 중력을 비활성화
            playerCollider.isTrigger = true;
            playerRigidbody.useGravity = false;
        }
        else
        {
            particleSystem.Stop();
            Debug.Log("파티클이 비활성화되었습니다.");
            PlayerBody.SetActive(true);
            PlayerWeapon.SetActive(true);

            // Monster 태그의 Collider 및 Player Collider와의 충돌 다시 활성화
            SetCollisionWithMonsters(true);

            // Player의 Collider를 일반 Collider로 되돌리고 중력을 활성화
            playerCollider.isTrigger = false;
            playerRigidbody.useGravity = true;
        }
    }

    // Monster 태그의 Collider와의 충돌 무시 설정 메서드
    private void SetCollisionWithMonsters(bool enableCollision)
    {
        Collider skillCollider = GetComponent<Collider>(); // 현재 객체의 Collider
        if (skillCollider == null) return;

        // Monster와의 충돌 무시
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
                Debug.Log($"{other.name}에게 {damageAmount}의 데미지를 입혔습니다. OnParticleCollision 발생");
            }
            else
            {
                Debug.LogWarning($"{other.name}에 MonsterStats가 없습니다.");
            }

            // 부드럽게 밀어내기
            if (monsterRigidbody != null)
            {
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                monsterRigidbody.AddForce(knockbackDirection * 200f, ForceMode.VelocityChange); // 부드럽게 밀어내는 힘
            }
        }
        else
        {
            Debug.Log("OnParticleCollision이 발생하지 않음. 파티클 활성 상태: " + isParticleActive);
        }
    }
}
