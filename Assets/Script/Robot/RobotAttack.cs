using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RobotAttack : MonoBehaviour
{
    public float detectionRange = 10f;  // �� Ž�� ����
    public float attackRange = 2f;      // ���� ����
    public float patrolRadius = 10f;    // ���� ����
    public float patrolUpdateInterval = 2f; // ���� ��ġ ���� �ֱ�
    public float punchInterval = 2f;    // ���� ����
    public ParticleSystem attackParticle; // ���� ��ƼŬ �ý���
    public Transform particleSpawnPoint; // ��ƼŬ �߻� ��ġ

    private NavMeshAgent agent;
    private GameObject targetMonster; // Ÿ�� ����
    private Vector3 originalPosition; // �ʱ� ��ġ
    private bool isAttacking = false; // ���� ���� ������ Ȯ��

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;

        StartCoroutine(BehaviorRoutine());  // ���� Ž�� �� �ൿ ��ƾ ����
        StartCoroutine(PatrolRoutine());    // ���� ��ƾ ����
    }

    private IEnumerator BehaviorRoutine()
    {
        while (true)
        {
            FindNearestMonster(); // ���� ����� ���� Ž��

            if (targetMonster != null && Vector3.Distance(transform.position, targetMonster.transform.position) <= attackRange)
            {
                if (!isAttacking)
                {
                    Debug.Log("���� ���� �� ���� �߰�, ���� ����!");
                    StartCoroutine(AttackCycle()); // ���� �ֱ� ����
                }
            }
            else if (targetMonster != null)
            {
                Debug.Log("���͸� �߰��Ͽ� �̵� ��...");
                agent.SetDestination(targetMonster.transform.position); // ���Ϳ��� �̵�
            }

            yield return new WaitForSeconds(patrolUpdateInterval); // ���� �� �ൿ ��ƾ ���
        }
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            if (agent.remainingDistance <= 0.5f || !agent.hasPath)
            {
                SetRandomPatrolLocation(); // ���ο� ���� ��ġ ����
            }
            yield return new WaitForSeconds(patrolUpdateInterval); // ���� �ֱ� ���
        }
    }

    private void SetRandomPatrolLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += originalPosition;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            Debug.Log($"�� ���� ��ġ ����: {hit.position}");
            agent.SetDestination(hit.position);
        }
    }

    private void FindNearestMonster()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        float minDistance = Mathf.Infinity;
        GameObject nearestMonster = null;

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Monster"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                Debug.Log($"���� �߰�: {collider.gameObject.name}, �Ÿ�: {distance}");

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestMonster = collider.gameObject;
                }
            }
        }

        targetMonster = nearestMonster;
    }

    private IEnumerator AttackCycle()
    {
        isAttacking = true;  // ���� �� ���� ����

        while (targetMonster != null && Vector3.Distance(transform.position, targetMonster.transform.position) <= attackRange)
        {
            // ��ƼŬ �߻� ��ġ�� Ÿ���� ���� ����
            particleSpawnPoint.LookAt(targetMonster.transform);
            attackParticle.transform.position = particleSpawnPoint.position;
            attackParticle.transform.rotation = particleSpawnPoint.rotation;

            // ũ��� ���̰� ������ ���·� ��ƼŬ �߻�
            var mainModule = attackParticle.main;
            mainModule.startSizeXMultiplier = 1.0f; // ���ϴ� ������ X ũ�� ����
            mainModule.startSizeYMultiplier = 1.0f; // ���ϴ� ������ Y ũ�� ����
            mainModule.startSizeZMultiplier = 1.0f; // ���ϴ� ������ Z ũ�� ����

            attackParticle.Play();  // ��ƼŬ Ȱ��ȭ
            Debug.Log($"{targetMonster.name}���� ��ƼŬ �߻�!");

            yield return new WaitForSeconds(2f); // 2�� ���� ���� ����

            attackParticle.Stop();  // ��ƼŬ ��Ȱ��ȭ
            Debug.Log("��ƼŬ ��Ȱ��ȭ");

            yield return new WaitForSeconds(2f); // 2�� ���� ���� ���
        }

        isAttacking = false;  // ���� �ֱ� ����
    }
}
