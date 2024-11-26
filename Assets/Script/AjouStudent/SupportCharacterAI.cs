using System.Collections;
using UnityEngine;

public class SupportCharacterAI : MonoBehaviour
{
    public enum AIState { Idle, Follow, Attack }
    private AIState currentState;

    [Header("References")]
    public Transform player;           // �÷��̾ ����
    public Animator animator;          // ĳ���� �ִϸ�����
    public Transform attackTarget;     // ���� ���� ���
    public LayerMask monsterLayer;     // ���� ���̾�

    [Header("Movement")]
    public float followDistance = 5f;  // �÷��̾���� �Ÿ�
    public float speed = 3f;

    [Header("Attack")]
    public float attackRange = 2f;     // ���� ����
    public float attackCooldown = 1f; // ���� ��ٿ�
    private bool canAttack = true;

    [Header("Teleport Settings")]
    public float maxDistanceFromPlayer = 20f; // �ִ� ��� �Ÿ�
    public float teleportOffset = 2f;         // �����̵� �� �÷��̾���� �Ÿ�

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

        // �÷��̾ ����
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

        // ���� ���� ������ ���� ����
        if (distanceToTarget <= attackRange && canAttack)
        {
            StartCoroutine(AttackCoroutine());
        }
        else if (distanceToTarget > attackRange)
        {
            currentState = AIState.Follow; // ������ ����� ����
        }
    }

    private IEnumerator AttackCoroutine()
    {
        canAttack = false;

        // ���� �ִϸ��̼� ���
        animator.SetTrigger("attack");

        // ���� ���� ����
        if (attackTarget != null && Vector3.Distance(transform.position, attackTarget.position) <= attackRange)
        {
            Debug.Log($"{gameObject.name} attacks {attackTarget.name}");
            MonsterStats monster = attackTarget.GetComponent<MonsterStats>();
            if (monster != null)
            {
                monster.TakeDamage(10); // ������ ó��
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �ִ� ��� �Ÿ� �ʰ� �� �����̵�
        if (distanceToPlayer > maxDistanceFromPlayer)
        {
            Vector3 teleportPosition = player.position + (Vector3.back * teleportOffset);
            transform.position = teleportPosition;
            Debug.Log($"{gameObject.name}��(��) �÷��̾� ��ó�� �����̵��߽��ϴ�.");
        }

        // ���� Ž��
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
