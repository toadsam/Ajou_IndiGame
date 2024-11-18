using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform player;                 // 플레이어 위치
    public float detectionRange = 10f;       // 플레이어를 감지하는 범위
    public float attackRange = 3f;           // 공격 범위
    public float idleMoveRadius = 5f;        // 몬스터가 돌아다니는 반경
    public int maxHealth = 100;              // 몬스터 최대 체력
    public int attackDamage = 10;            // 몬스터 공격력
    public float idleDuration = 3f;          // Idle 상태 유지 시간
    public float attackCooldown = 2f;        // 공격 쿨타임
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

        // 플레이어 오브젝트 찾기
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
                // 피해 애니메이션 중에는 다른 동작을 하지 않음
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
                // 공격 범위 안에 플레이어가 있으면 데미지 입힘
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
        yield return new WaitForSeconds(0.5f); // 피격 후 대기 시간
        currentState = AIState.Chase;
    }

    private void Die()
    {
        animator.SetTrigger("isDead");
        agent.isStopped = true;
        Destroy(gameObject, 2f); // 2초 후에 몬스터 오브젝트 삭제
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
