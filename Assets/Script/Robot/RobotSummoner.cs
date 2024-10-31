using System.Collections;
using UnityEngine;

public class RobotSummoner : MonoBehaviour
{
    public GameObject robotPrefab; // �κ� ������
    public Transform playerTransform; // �÷��̾� ��ġ ����
    public float summonDuration = 20f; // ��ȯ�� �κ��� ���� �ð�
    public float cooldownTime = 20f; // ��ų ���� ��� �ð�

    private bool isOnCooldown = false; // ��ų ��� ���� Ȯ��

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isOnCooldown)
        {
            StartCoroutine(SummonRobot()); // Q ��ư���� �κ� ��ȯ
        }
    }

    private IEnumerator SummonRobot()
    {
        isOnCooldown = true;

        // �÷��̾� ���ʿ� �κ� ����
        Vector3 summonPosition = playerTransform.position + playerTransform.forward * 2f;
        GameObject robot = Instantiate(robotPrefab, summonPosition, Quaternion.identity);

        Debug.Log("�κ� ��ȯ��"); // �κ� ��ȯ Ȯ��

        // �κ��� summonDuration(�⺻�� 10��) ���� ������ �� �����
        yield return new WaitForSeconds(summonDuration);

        Debug.Log("�κ� �����"); // �κ� ����� Ȯ��
        Destroy(robot);

        // ��Ÿ�� ����
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;

        Debug.Log("��ų ��Ÿ�� ����"); // ��Ÿ�� ���� Ȯ��
    }
}
