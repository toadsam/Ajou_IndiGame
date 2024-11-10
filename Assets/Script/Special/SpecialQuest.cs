using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpecialQuest : MonoBehaviour
{
    public GameObject player;                    // Player ������Ʈ
    public GameObject missionTarget;             // �̼� Ÿ�� ������Ʈ
    public GameObject monsterPrefab;             // ���� ������
    public Transform[] randomPositions;          // ���� ��ġ �迭
    public GameObject upgradeUI;                 // �Ӽ� ���� UI
    public Button speedButton, attackButton, defenseButton;

    private bool isMissionActive = false;
    private float timer = 30f;

    private enum MissionType { FindObject, ReachMonster, KillMonsters }
    private MissionType currentMission;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // �Ӽ� ���� ��ư Ŭ�� �̺�Ʈ
        speedButton.onClick.AddListener(() => UpgradePlayer("speed"));
        attackButton.onClick.AddListener(() => UpgradePlayer("attack"));
        defenseButton.onClick.AddListener(() => UpgradePlayer("defense"));

        upgradeUI.SetActive(false); // �Ӽ� ���� UI�� ó���� ��Ȱ��ȭ
    }

    private void Update()
    {
        if (!isMissionActive)
        {
            timer -= Time.deltaTime;
            Debug.Log($"�̼� ��� �ð�: {timer:F2}�� ����");

            if (timer <= 0f)
            {
                Debug.Log("Player�� ��ȣ�ۿ����� �ʾ� NPC�� �ı��˴ϴ�.");
                Destroy(gameObject);  // �÷��̾ NPC�� ��ȣ�ۿ����� ������ NPC �ı�
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isMissionActive && other.CompareTag("Player"))
        {
            Debug.Log("Player�� NPC�� �����Ͽ� �̼��� �����մϴ�.");
            StartCoroutine(StartRandomMission());
            isMissionActive = true;
        }
    }

    private IEnumerator StartRandomMission()
    {
        currentMission = (MissionType)Random.Range(0, 3);
        Debug.Log($"���� �̼� ����: {currentMission}");

        switch (currentMission)
        {
            case MissionType.FindObject:
                Vector3 targetPosition = randomPositions[Random.Range(0, randomPositions.Length)].position;
                missionTarget.transform.position = targetPosition;
                missionTarget.SetActive(true);
                Debug.Log("�̼�: ������ ��ġ�� �̵��Ͽ� ������Ʈ�� �����ϱ�");

                yield return StartCoroutine(MissionTimer(10f, () => Vector3.Distance(player.transform.position, missionTarget.transform.position) < 1f));
                missionTarget.SetActive(false);
                break;

            case MissionType.ReachMonster:
                GameObject monster = Instantiate(monsterPrefab, randomPositions[Random.Range(0, randomPositions.Length)].position, Quaternion.identity);
                Debug.Log("�̼�: ��ȯ�� ���Ϳ� 12�� ���� �����ϱ�");

                yield return StartCoroutine(MissionTimer(12f, () => Vector3.Distance(player.transform.position, monster.transform.position) < 1f));
                Destroy(monster);
                break;

            case MissionType.KillMonsters:
                Debug.Log("�̼�: 20�� ���� ��ȯ�� ���� 20���� óġ�ϱ�");
                for (int i = 0; i < 20; i++)
                {
                    Instantiate(monsterPrefab, randomPositions[Random.Range(0, randomPositions.Length)].position, Quaternion.identity);
                }

                yield return StartCoroutine(MissionTimer(20f, () => GameObject.FindGameObjectsWithTag("Monster").Length == 0));
                break;
        }

        if (MissionSuccess())
        {
            Debug.Log("�̼� ����! ���� ���� UI�� ǥ���մϴ�.");
            upgradeUI.SetActive(true);
        }
        else
        {
            Debug.Log("�̼� ����!");
        }
    }

    private IEnumerator MissionTimer(float duration, System.Func<bool> successCondition)
    {
        float missionTimer = duration;
        Debug.Log($"�̼� Ÿ�̸� ����: {duration}��");

        while (missionTimer > 0f)
        {
            missionTimer -= Time.deltaTime;
            Debug.Log($"�̼� ���� ��... ���� �ð�: {missionTimer:F2}��");

            if (successCondition.Invoke())
            {
                Debug.Log("�̼� ���� ���� ����");
                yield break;
            }

            yield return null;
        }
        Debug.Log("�̼� �ð� �ʰ�");
    }

    private bool MissionSuccess()
    {
        switch (currentMission)
        {
            case MissionType.FindObject:
                bool foundObject = Vector3.Distance(player.transform.position, missionTarget.transform.position) < 1f;
                Debug.Log(foundObject ? "������Ʈ�� ���� �Ϸ�" : "������Ʈ�� ���� ����");
                return foundObject;

            case MissionType.ReachMonster:
                bool reachedMonster = GameObject.FindGameObjectsWithTag("Monster").Length == 0;
                Debug.Log(reachedMonster ? "���� ���� �Ϸ�" : "���� ���� ����");
                return reachedMonster;

            case MissionType.KillMonsters:
                bool killedAllMonsters = GameObject.FindGameObjectsWithTag("Monster").Length == 0;
                Debug.Log(killedAllMonsters ? "��� ���� óġ �Ϸ�" : "��� ���� óġ ����");
                return killedAllMonsters;

            default:
                return false;
        }
    }

    private void UpgradePlayer(string attribute)
    {
        Debug.Log($"{attribute} �Ӽ��� �����߽��ϴ�.");

        switch (attribute)
        {
            case "speed":
                player.GetComponent<PlayerStats>().strength += 1; //�ϴ��� ������ �Ұ�
                Debug.Log("Player �ӵ��� �����߽��ϴ�.");
                break;
            case "attack":
                player.GetComponent<PlayerStats>().strength += 1;
                Debug.Log("Player ���ݷ��� �����߽��ϴ�.");
                break;
            case "defense":
                player.GetComponent<PlayerStats>().defense += 1;
                Debug.Log("Player ������ �����߽��ϴ�.");
                break;
        }

        upgradeUI.SetActive(false);
    }
}
