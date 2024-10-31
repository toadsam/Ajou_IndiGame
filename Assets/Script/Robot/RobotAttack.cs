using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RobotAttack : MonoBehaviour
{
    public float detectionRange = 10f;  // 적 탐지 범위
    public float attackRange = 2f;      // 공격 범위
    public float patrolRadius = 10f;    // 순찰 범위
    public float patrolUpdateInterval = 2f; // 순찰 위치 갱신 주기
    public float punchInterval = 2f;    // 공격 간격
    public ParticleSystem attackParticle; // 공격 파티클 시스템
    public Transform particleSpawnPoint; // 파티클 발사 위치

    private NavMeshAgent agent;
    private GameObject targetMonster; // 타겟 몬스터
    private Vector3 originalPosition; // 초기 위치
    private bool isAttacking = false; // 현재 공격 중인지 확인

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;

        StartCoroutine(BehaviorRoutine());  // 몬스터 탐지 및 행동 루틴 시작
        StartCoroutine(PatrolRoutine());    // 순찰 루틴 시작
    }

    private IEnumerator BehaviorRoutine()
    {
        while (true)
        {
            FindNearestMonster(); // 가장 가까운 몬스터 탐색

            if (targetMonster != null && Vector3.Distance(transform.position, targetMonster.transform.position) <= attackRange)
            {
                if (!isAttacking)
                {
                    Debug.Log("공격 범위 내 몬스터 발견, 공격 시작!");
                    StartCoroutine(AttackCycle()); // 공격 주기 시작
                }
            }
            else if (targetMonster != null)
            {
                Debug.Log("몬스터를 발견하여 이동 중...");
                agent.SetDestination(targetMonster.transform.position); // 몬스터에게 이동
            }

            yield return new WaitForSeconds(patrolUpdateInterval); // 순찰 및 행동 루틴 대기
        }
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            if (agent.remainingDistance <= 0.5f || !agent.hasPath)
            {
                SetRandomPatrolLocation(); // 새로운 순찰 위치 설정
            }
            yield return new WaitForSeconds(patrolUpdateInterval); // 순찰 주기 대기
        }
    }

    private void SetRandomPatrolLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += originalPosition;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            Debug.Log($"새 순찰 위치 설정: {hit.position}");
            agent.SetDestination(hit.position);
        }
    }

    private void FindNearestMonster()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        float minDistance = Mathf.Infinity;
        GameObject nearestMonster = null;

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Monster"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                Debug.Log($"몬스터 발견: {collider.gameObject.name}, 거리: {distance}");

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestMonster = collider.gameObject;
                }
            }
        }

        targetMonster = nearestMonster;
    }

    private IEnumerator AttackCycle()
    {
        isAttacking = true;  // 공격 중 상태 설정

        while (targetMonster != null && Vector3.Distance(transform.position, targetMonster.transform.position) <= attackRange)
        {
            // 파티클 발사 위치를 타겟을 향해 조정
            particleSpawnPoint.LookAt(targetMonster.transform);
            attackParticle.transform.position = particleSpawnPoint.position;
            attackParticle.transform.rotation = particleSpawnPoint.rotation;

            // 크기와 길이가 고정된 상태로 파티클 발사
            var mainModule = attackParticle.main;
            mainModule.startSizeXMultiplier = 1.0f; // 원하는 고정된 X 크기 설정
            mainModule.startSizeYMultiplier = 1.0f; // 원하는 고정된 Y 크기 설정
            mainModule.startSizeZMultiplier = 1.0f; // 원하는 고정된 Z 크기 설정

            attackParticle.Play();  // 파티클 활성화
            Debug.Log($"{targetMonster.name}에게 파티클 발사!");

            yield return new WaitForSeconds(2f); // 2초 동안 공격 유지

            attackParticle.Stop();  // 파티클 비활성화
            Debug.Log("파티클 비활성화");

            yield return new WaitForSeconds(2f); // 2초 동안 공격 대기
        }

        isAttacking = false;  // 공격 주기 종료
    }
}
