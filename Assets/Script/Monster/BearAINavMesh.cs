using UnityEngine;
using UnityEngine.AI;

public class BearAINavMesh : MonoBehaviour
{
    public Transform player; // 플레이어의 위치
    public float attackRange = 3f; // 공격 범위
    public float dodgeRange = 5f; // 회피 범위
    public float attackCooldown = 2f; // 공격 후 대기 시간

    private float attackTimer = 0f; // 공격 쿨타임
    private NavMeshAgent agent; // NavMesh 에이전트
    private Vector3 dodgePosition; // 회피 위치
    private Animator animator; // 애니메이터

    enum AIState { Idle, Attack, Dodge }
    AIState currentState = AIState.Idle;

    void Start()
    {
        // NavMeshAgent와 Animator 컴포넌트 가져오기
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // 애니메이터 연결
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        switch (currentState)
        {
            case AIState.Idle:
                MoveTowardsPlayer();
                CheckForStateChange();
                break;
            case AIState.Attack:
                Attack();
                break;
            case AIState.Dodge:
                Dodge();
                break;
        }
    }

    // 플레이어를 향해 이동 (네비 메쉬 사용)
    void MoveTowardsPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            agent.SetDestination(player.position); // 네비 메쉬를 사용해 플레이어로 이동
            animator.SetBool("isMoving", true); // 이동 애니메이션 재생
        }
        else
        {
            animator.SetBool("isMoving", false); // 이동 중지
        }
    }

    // 상태 전환을 위한 조건 체크
    void CheckForStateChange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 회피할 조건 체크 (플레이어가 너무 가까워질 때)
        if (distanceToPlayer < dodgeRange && attackTimer >= attackCooldown)
        {
            currentState = AIState.Dodge;
            SetDodgePosition();
        }
        // 공격할 조건 체크
        else if (distanceToPlayer < attackRange && attackTimer >= attackCooldown)
        {
            currentState = AIState.Attack;
        }
    }

    // 회피할 위치 설정 (플레이어로부터 멀어지는 방향)
    void SetDodgePosition()
    {
        Vector3 direction = (transform.position - player.position).normalized;
        dodgePosition = transform.position + direction * dodgeRange; // 플레이어 반대쪽으로 이동
        NavMeshHit hit;

        // 네비 메쉬에서 회피 위치가 유효한지 확인
        if (NavMesh.SamplePosition(dodgePosition, out hit, dodgeRange, NavMesh.AllAreas))
        {
            dodgePosition = hit.position;
        }
    }

    // 회피 동작
    void Dodge()
    {
        agent.SetDestination(dodgePosition);
        animator.SetTrigger("isDodging"); // 회피 애니메이션 재생

        if (Vector3.Distance(transform.position, dodgePosition) < 0.5f)
        {
            currentState = AIState.Idle;
        }
    }

    // 공격 동작
    void Attack()
    {
        agent.isStopped = true; // 공격 중 이동 멈춤
        animator.SetTrigger("isAttacking"); // 공격 애니메이션 재생

        // 공격 후 쿨타임 설정
        attackTimer = 0f;
        currentState = AIState.Idle;
        agent.isStopped = false; // 공격 후 이동 재개
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // 공격 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, dodgeRange); // 회피 범위
    }
}
