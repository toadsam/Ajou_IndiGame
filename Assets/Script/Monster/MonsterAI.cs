using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform player;                 // �÷��̾� ��ġ
    public float detectionRange = 10f;       // �÷��̾ �����ϴ� ����
    public float attackRange = 3f;           // ���� ����
    public float idleMoveRadius = 5f;        // ���Ͱ� ���ƴٴϴ� �ݰ�
    public int maxHealth = 100;              // ���� �ִ� ü��
    public int attackDamage = 10;            // ���� ���ݷ�
    public float idleDuration = 3f;          // Idle ���� ���� �ð�
    public float attackCooldown = 2f;        // ���� ��Ÿ��
    private int currentHealth;

    private float attackTimer;
    private float idleTimer;
    private Vector3 idleTarget;
    private NavMeshAgent agent;
    private Animator animator;

    private enum AIState { Idle, Move, Chase, Attack, TakeDamage, Die }
    private AIState currentState = AIState.Idle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        // �÷��̾� ������Ʈ ã��
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Idle:
                Idle();
                break;
            case AIState.Move:
                Move();
                break;
            case AIState.Chase:
                ChasePlayer();
                break;
            case AIState.Attack:
                AttackPlayer();
                break;
            case AIState.TakeDamage:
                // ���� �ִϸ��̼� �߿��� �ٸ� ������ ���� ����
                break;
            case AIState.Die:
                Die();
                break;
        }
    }

    private void Idle()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration)
        {
            idleTimer = 0f;
            SetNewIdleTarget();
            currentState = AIState.Move;
        }

        if (IsPlayerInRange(detectionRange))
        {
            currentState = AIState.Chase;
        }
    }

    private void Move()
    {
        agent.SetDestination(idleTarget);
        animator.SetBool("isMoving", true);

        if (Vector3.Distance(transform.position, idleTarget) < 0.5f)
        {
            currentState = AIState.Idle;
            animator.SetBool("isMoving", false);
        }

        if (IsPlayerInRange(detectionRange))
        {
            currentState = AIState.Chase;
            animator.SetBool("isMoving", false);
        }
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        agent.SetDestination(player.position);
        animator.SetBool("isMoving", true);

        if (IsPlayerInRange(attackRange))
        {
            currentState = AIState.Attack;
            animator.SetBool("isMoving", false);
        }
        else if (!IsPlayerInRange(detectionRange))
        {
            currentState = AIState.Idle;
            animator.SetBool("isMoving", false);
        }
    }

    private void AttackPlayer()
    {
        if (attackTimer >= attackCooldown)
        {
            animator.SetTrigger("isAttacking");
            attackTimer = 0f;

            if (IsPlayerInRange(attackRange))
            {
                // ���� ���� �ȿ� �÷��̾ ������ ������ ����
                player.GetComponent<PlayerStats>().TakeDamage(attackDamage);
            }
        }
        else
        {
            attackTimer += Time.deltaTime;
        }

        if (!IsPlayerInRange(attackRange))
        {
            currentState = AIState.Chase;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentState == AIState.Die) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentState = AIState.Die;
        }
        else
        {
            currentState = AIState.TakeDamage;
            animator.SetTrigger("isHit");
            StartCoroutine(RecoverFromHit());
        }
    }

    private IEnumerator RecoverFromHit()
    {
        yield return new WaitForSeconds(0.5f); // �ǰ� �� ��� �ð�
        currentState = AIState.Chase;
    }

    private void Die()
    {
        animator.SetTrigger("isDead");
        agent.isStopped = true;
        Destroy(gameObject, 2f); // 2�� �Ŀ� ���� ������Ʈ ����
    }

    private void SetNewIdleTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * idleMoveRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, idleMoveRadius, NavMesh.AllAreas))
        {
            idleTarget = hit.position;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.position) <= range;
    }
}
