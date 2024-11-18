using UnityEngine;
using UnityEngine.AI;

public class ExperimentalPerson : MonoBehaviour
{
    public Transform player;
    public float attackRange = 3f;
    public float dodgeRange = 5f;
    public float attackCooldown = 2f;
    public int attackDamage = 10;

    private float attackTimer = 0f;
    private NavMeshAgent agent;
    private Vector3 dodgePosition;
    private Animator animator;
    private PlayerStats playerStats;

    private bool isDead = false;

    enum AIState { Idle, Attack, Dodge }
    AIState currentState = AIState.Idle;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerStats = playerObject.GetComponent<PlayerStats>();
        }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

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

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            if (playerStats != null)
            {
                playerStats.TakeDamage(attackDamage);
            }
        }

        attackTimer = 0f;
        currentState = AIState.Idle;
        agent.isStopped = false;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("isDead"); // 죽는 애니메이션 트리거

        // 애니메이션이 끝난 후 풀로 반환
        Invoke(nameof(ReturnToPool), 2f); // 애니메이션 길이에 맞춰서 시간 조정
    }

    private void ReturnToPool()
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
