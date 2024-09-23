using UnityEngine;
using UnityEngine.AI;

public class BearAINavMesh : MonoBehaviour
{
    public Transform player; // �÷��̾��� ��ġ
    public float attackRange = 3f; // ���� ����
    public float dodgeRange = 5f; // ȸ�� ����
    public float attackCooldown = 2f; // ���� �� ��� �ð�

    private float attackTimer = 0f; // ���� ��Ÿ��
    private NavMeshAgent agent; // NavMesh ������Ʈ
    private Vector3 dodgePosition; // ȸ�� ��ġ
    private Animator animator; // �ִϸ�����

    enum AIState { Idle, Attack, Dodge }
    AIState currentState = AIState.Idle;

    void Start()
    {
        // NavMeshAgent�� Animator ������Ʈ ��������
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // �ִϸ����� ����
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

    // �÷��̾ ���� �̵� (�׺� �޽� ���)
    void MoveTowardsPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            agent.SetDestination(player.position); // �׺� �޽��� ����� �÷��̾�� �̵�
            animator.SetBool("isMoving", true); // �̵� �ִϸ��̼� ���
        }
        else
        {
            animator.SetBool("isMoving", false); // �̵� ����
        }
    }

    // ���� ��ȯ�� ���� ���� üũ
    void CheckForStateChange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // ȸ���� ���� üũ (�÷��̾ �ʹ� ������� ��)
        if (distanceToPlayer < dodgeRange && attackTimer >= attackCooldown)
        {
            currentState = AIState.Dodge;
            SetDodgePosition();
        }
        // ������ ���� üũ
        else if (distanceToPlayer < attackRange && attackTimer >= attackCooldown)
        {
            currentState = AIState.Attack;
        }
    }

    // ȸ���� ��ġ ���� (�÷��̾�κ��� �־����� ����)
    void SetDodgePosition()
    {
        Vector3 direction = (transform.position - player.position).normalized;
        dodgePosition = transform.position + direction * dodgeRange; // �÷��̾� �ݴ������� �̵�
        NavMeshHit hit;

        // �׺� �޽����� ȸ�� ��ġ�� ��ȿ���� Ȯ��
        if (NavMesh.SamplePosition(dodgePosition, out hit, dodgeRange, NavMesh.AllAreas))
        {
            dodgePosition = hit.position;
        }
    }

    // ȸ�� ����
    void Dodge()
    {
        agent.SetDestination(dodgePosition);
        animator.SetTrigger("isDodging"); // ȸ�� �ִϸ��̼� ���

        if (Vector3.Distance(transform.position, dodgePosition) < 0.5f)
        {
            currentState = AIState.Idle;
        }
    }

    // ���� ����
    void Attack()
    {
        agent.isStopped = true; // ���� �� �̵� ����
        animator.SetTrigger("isAttacking"); // ���� �ִϸ��̼� ���

        // ���� �� ��Ÿ�� ����
        attackTimer = 0f;
        currentState = AIState.Idle;
        agent.isStopped = false; // ���� �� �̵� �簳
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // ���� ����
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, dodgeRange); // ȸ�� ����
    }
}
