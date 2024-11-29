using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class BossMonsterAI : MonoBehaviour
{
    public enum BossState
    {
        Idle,
        Walk,
        Attack1,
        Attack2,
        Attack3
    }

    [Header("Boss Settings")]
    public float walkSpeed = 3f;
    public float attackRange = 5f;
    public float chaseRange = 15f;
    public float attackCooldown = 2f;

    [Header("Animation and AI")]
    private Animator animator;
    private NavMeshAgent navAgent;
    private Transform playerTransform;

    [Header("Attack Settings")]
    public int attackDamage1 = 10; // ������ 1
    public int attackDamage2 = 20; // ������ 2
    public int attackDamage3 = 30; // ������ 3

    [Header("Attack3 Settings")]
    public GameObject[] attack3Prefabs; // Attack3���� ������ �پ��� ������Ʈ �迭
    public int attack3SpawnCount = 5;   // ������ ������Ʈ ����
    public float attack3Lifetime = 5f; // ������ ������Ʈ ���� �ð�

    private BossState currentState = BossState.Idle;
    private bool canAttack = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent != null)
        {
            navAgent.speed = walkSpeed;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the Player object has the 'Player' tag.");
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        switch (currentState)
        {
            case BossState.Idle:
                HandleIdleState(distanceToPlayer);
                break;
            case BossState.Walk:
                HandleWalkState(distanceToPlayer);
                break;
            case BossState.Attack1:
            case BossState.Attack2:
            case BossState.Attack3:
                // ���� ���´� �ִϸ��̼� �̺�Ʈ�� ���� ó����
                break;
        }

        UpdateAnimatorParameters(distanceToPlayer);
    }

    private void HandleIdleState(float distanceToPlayer)
    {
        navAgent.isStopped = true;

        if (distanceToPlayer <= chaseRange)
        {
            currentState = BossState.Walk;
        }
    }

    private void HandleWalkState(float distanceToPlayer)
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(playerTransform.position);

        if (distanceToPlayer <= attackRange && canAttack)
        {
            StartCoroutine(PerformRandomAttack());
        }
        else if (distanceToPlayer > chaseRange)
        {
            currentState = BossState.Idle;
        }
    }

    private IEnumerator PerformRandomAttack()
    {
        canAttack = false;

        // ���� �� �̵� ����
        navAgent.isStopped = true;

        // �������� ���� ����
        int attackChoice = Random.Range(1, 4);
        currentState = (BossState)attackChoice;
        animator.SetTrigger($"Attack{attackChoice}");

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
        currentState = BossState.Walk;

        // �̵� �簳
        navAgent.isStopped = false;
    }

    private void UpdateAnimatorParameters(float distanceToPlayer)
    {
        animator.SetBool("isIdle", currentState == BossState.Idle);
        animator.SetBool("isWalking", currentState == BossState.Walk);
    }

    // �ִϸ��̼� �̺�Ʈ: ���� ����
    public void PerformAttack1()
    {
        Debug.Log("Performing Attack1");
        DealDamage(attackDamage1);
    }

    public void PerformAttack2()
    {
        Debug.Log("Performing Attack2");
        DealDamage(attackDamage2);
    }

    public void PerformAttack3()
    {
        Debug.Log("Performing Attack3: Summoning Objects");
        StartCoroutine(SpawnAttack3Objects());
    }

    private void DealDamage(int damage)
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= attackRange)
        {
            PlayerStats playerStats = playerTransform.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
                Debug.Log($"Boss dealt {damage} damage to the player.");
            }
        }
    }

    private IEnumerator SpawnAttack3Objects()
    {
        if (attack3Prefabs == null || attack3Prefabs.Length == 0)
        {
            Debug.LogWarning("Attack3 Prefabs are not assigned.");
            yield break;
        }

        List<GameObject> spawnedObjects = new List<GameObject>();

        for (int i = 0; i < attack3SpawnCount; i++)
        {
            // ���� ������Ʈ ����
            GameObject prefab = attack3Prefabs[Random.Range(0, attack3Prefabs.Length)];
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * 3f;
            randomPosition.y = transform.position.y;

            // ������Ʈ ����
            GameObject obj = Instantiate(prefab, randomPosition, Quaternion.identity);
            spawnedObjects.Add(obj);
        }

        yield return new WaitForSeconds(attack3Lifetime);

        // ������ ������Ʈ ����
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }

    // �ִϸ��̼� �̺�Ʈ: ���� ����
    public void EndAttack()
    {
        if (currentState == BossState.Attack1 || currentState == BossState.Attack2 || currentState == BossState.Attack3)
        {
            currentState = BossState.Walk;
        }
    }
}
