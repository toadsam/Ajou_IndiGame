using UnityEngine;
using UnityEngine.AI;

public class Monstor : MonoBehaviour
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
        // 플레이어 오브젝트 찾기
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // NavMeshAgent와 Animator 컴포넌트 가져오기
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            agent.SetDestination(player.position);
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    void CheckForStateChange()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < dodgeRange && attackTimer >= attackCooldown)
        {
            currentState = AIState.Dodge;
            SetDodgePosition();
        }
        else if (distanceToPlayer < attackRange && attackTimer >= attackCooldown)
        {
            currentState = AIState.Attack;
        }
    }

    void SetDodgePosition()
    {
        if (player == null) return;

        Vector3 direction = (transform.position - player.position).normalized;
        dodgePosition = transform.position + direction * dodgeRange;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(dodgePosition, out hit, dodgeRange, NavMesh.AllAreas))
        {
            dodgePosition = hit.position;
        }
    }

    void Dodge()
    {
        agent.SetDestination(dodgePosition);
        animator.SetTrigger("isDodging");

        if (Vector3.Distance(transform.position, dodgePosition) < 0.5f)
        {
            currentState = AIState.Idle;
        }
    }

    void Attack()
    {
        agent.isStopped = true;
        animator.SetTrigger("isAttacking");

        attackTimer = 0f;
        currentState = AIState.Idle;
        agent.isStopped = false;
    }

    // 몬스터가 죽을 때 풀로 반환하는 함수
    public void Die()
    {
        MonsterPoolManager.instance.ReturnMonster(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, dodgeRange);
    }
}
