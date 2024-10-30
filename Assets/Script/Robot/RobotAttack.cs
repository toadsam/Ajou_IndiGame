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

    private NavMeshAgent agent;
    private GameObject targetMonster; // 타겟 몬스터
    private Vector3 originalPosition; // 초기 위치

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;

        // NavMeshAgent 설정 확인
        Debug.Log($"NavMeshAgent 속도: {agent.speed}, 높이: {agent.height}, 반경: {agent.radius}");

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
                Debug.Log("공격 범위 내 몬스터 발견, 공격 시작!");
                Attack(); // 공격 실행
            }
            else if (targetMonster != null)
            {
                Debug.Log("몬스터를 발견하여 이동 중...");
                agent.SetDestination(targetMonster.transform.position); // 몬스터에게 이동
            }

            yield return new WaitForSeconds(punchInterval); // 행동 루틴 대기
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
            else
            {
                Debug.Log($"이동 중... 남은 거리: {agent.remainingDistance}");
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
        else
        {
            Debug.LogWarning("유효한 순찰 위치를 찾지 못했습니다.");
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
        if (nearestMonster != null)
            Debug.Log($"가장 가까운 몬스터: {nearestMonster.name}");
        else
            Debug.Log("탐지된 몬스터가 없습니다.");
    }

    private void Attack()
    {
        if (attackParticle != null && targetMonster != null)
        {
            agent.isStopped = true;
            attackParticle.transform.position = targetMonster.transform.position;
            attackParticle.Play();
            Debug.Log($"공격 중: {targetMonster.name}");
        }
    }
}
