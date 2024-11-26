using System.Collections;
using UnityEngine;

public class SupportCharacterAI : MonoBehaviour
{
    public enum AIState { Idle, Follow, Attack }
    private AIState currentState;

    [Header("References")]
    public Transform player;           // 플레이어를 따라감
    public Animator animator;          // 캐릭터 애니메이터
    public Transform attackTarget;     // 현재 공격 대상
    public LayerMask monsterLayer;     // 몬스터 레이어

    [Header("Movement")]
    public float followDistance = 5f;  // 플레이어와의 거리
    public float speed = 3f;

    [Header("Attack")]
    public float attackRange = 2f;     // 공격 범위
    public float attackCooldown = 1f; // 공격 쿨다운
    private bool canAttack = true;

    [Header("Teleport Settings")]
    public float maxDistanceFromPlayer = 20f; // 최대 허용 거리
    public float teleportOffset = 2f;         // 순간이동 시 플레이어와의 거리

    private void Start()
    {
        currentState = AIState.Idle;
    }

    private void Update()
    {
        switch (currentState)
        {
            case AIState.Idle:
                HandleIdle();
                break;
            case AIState.Follow:
                HandleFollow();
                break;
            case AIState.Attack:
                HandleAttack();
                break;
        }
        UpdateState();
    }

    private void HandleIdle()
    {
        animator.SetBool("isRunning", false);
    }

    private void HandleFollow()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어를 따라감
        if (distanceToPlayer > followDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            transform.LookAt(player);

            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void HandleAttack()
    {
        if (attackTarget == null)
        {
            currentState = AIState.Follow;
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, attackTarget.position);

        // 공격 범위 내에서 공격 실행
        if (distanceToTarget <= attackRange && canAttack)
        {
            StartCoroutine(AttackCoroutine());
        }
        else if (distanceToTarget > attackRange)
        {
            currentState = AIState.Follow; // 범위를 벗어나면 따라감
        }
    }

    private IEnumerator AttackCoroutine()
    {
        canAttack = false;

        // 공격 애니메이션 재생
        animator.SetTrigger("attack");

        // 실제 공격 로직
        if (attackTarget != null && Vector3.Distance(transform.position, attackTarget.position) <= attackRange)
        {
            Debug.Log($"{gameObject.name} attacks {attackTarget.name}");
            MonsterStats monster = attackTarget.GetComponent<MonsterStats>();
            if (monster != null)
            {
                monster.TakeDamage(10); // 데미지 처리
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 최대 허용 거리 초과 시 순간이동
        if (distanceToPlayer > maxDistanceFromPlayer)
        {
            Vector3 teleportPosition = player.position + (Vector3.back * teleportOffset);
            transform.position = teleportPosition;
            Debug.Log($"{gameObject.name}이(가) 플레이어 근처로 순간이동했습니다.");
        }

        // 몬스터 탐지
        Collider[] monstersInRange = Physics.OverlapSphere(transform.position, attackRange, monsterLayer);
        if (monstersInRange.Length > 0)
        {
            attackTarget = monstersInRange[0].transform;
            currentState = AIState.Attack;
        }
        else if (distanceToPlayer > followDistance)
        {
            currentState = AIState.Follow;
        }
        else
        {
            currentState = AIState.Idle;
        }
    }
}
