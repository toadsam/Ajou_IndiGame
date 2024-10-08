using UnityEngine;
using UnityEngine.AI;

public class Monstor : MonoBehaviour
{
    public Transform player; // �÷��̾��� ��ġ
    public float attackRange = 3f; // ���� ����
    public float dodgeRange = 5f; // ȸ�� ����
    public float attackCooldown = 2f; // ���� �� ��� �ð�
    public int attackDamage = 10; // ������ ���ݷ�

    private float attackTimer = 0f; // ���� ��Ÿ��
    private NavMeshAgent agent; // NavMesh ������Ʈ
    private Vector3 dodgePosition; // ȸ�� ��ġ
    private Animator animator; // �ִϸ�����

    private PlayerStats playerStats; // �÷��̾��� ü�� ���� ��ũ��Ʈ ����

    enum AIState { Idle, Attack, Dodge }
    AIState currentState = AIState.Idle;

    void Start()
    {
        // �÷��̾� ������Ʈ ã��
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerStats = playerObject.GetComponent<PlayerStats>(); // �÷��̾� ü�� ���� ��ũ��Ʈ ����
        }

        // NavMeshAgent�� Animator ������Ʈ ��������
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
            Debug.Log("��������");
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

    // ���� ����
    void Attack()
    {
        agent.isStopped = true;
        Debug.Log("���Ͱ� �����ϴ� �κп� ���Դ�");
        animator.SetTrigger("isAttacking");

        // ���� ���� ������ �÷��̾�� �������� ����
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            if (playerStats != null)
            {
                playerStats.TakeDamage(attackDamage); // �÷��̾��� ü�� ����
            }
        }

        attackTimer = 0f;
        currentState = AIState.Idle;
        agent.isStopped = false;
    }

    // ���Ͱ� ���� �� Ǯ�� ��ȯ�ϴ� �Լ�
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
