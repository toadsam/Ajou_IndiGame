using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpecialQuest : MonoBehaviour
{
    public GameObject missionTargetPrefab;        // �̼� Ÿ�� ������
    public GameObject monsterPrefab;             // ���� ������

    private GameObject player;                   // Player ������Ʈ
    private GameObject missionTarget;            // �̼� Ÿ�� �ν��Ͻ�
    private GameObject upgradeUI;                // �Ӽ� ���� UI �ν��Ͻ�
    private Button speedButton, attackButton, defenseButton;
    private Transform[] randomPositions;         // ���� ��ġ �迭

    private bool isMissionActive = false;
    private float timer = 30f;

    private enum MissionType { FindObject, ReachMonster, KillMonsters }
    private MissionType currentMission;

    private void Start()
    {
        // �±׷� ������Ʈ ã��
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        // ���� ��ġ �迭 �ʱ�ȭ
        GameObject randomPositionParent = GameObject.FindGameObjectWithTag("RandomPosition");
        if (randomPositionParent != null)
        {
            randomPositions = new Transform[randomPositionParent.transform.childCount];
            for (int i = 0; i < randomPositionParent.transform.childCount; i++)
            {
                randomPositions[i] = randomPositionParent.transform.GetChild(i);
            }
            Debug.Log("Random Positions �迭 �Ҵ� �Ϸ�");
        }
        else
        {
            Debug.LogError("RandomPosition �±��� ������Ʈ�� �����ϴ�.");
            return;
        }

        // Upgrade UI ã��
        upgradeUI = GameObject.FindGameObjectWithTag("UpgradeUI");
        if (upgradeUI == null)
        {
            Debug.LogError("UpgradeUI ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        upgradeUI.SetActive(true); // �ӽ� Ȱ��ȭ

        // Panel ���ο��� ��ư ã��
        Transform panel = upgradeUI.transform.Find("Panel");
        if (panel == null)
        {
            Debug.LogError("Panel ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        speedButton = panel.Find("SpeedButton")?.GetComponent<Button>();
        attackButton = panel.Find("AttackButton")?.GetComponent<Button>();
        defenseButton = panel.Find("DefenseButton")?.GetComponent<Button>();

        upgradeUI.SetActive(false); // �ٽ� ��Ȱ��ȭ

        if (speedButton == null || attackButton == null || defenseButton == null)
        {
            Debug.LogError("��ư �� �ϳ��� ã�� ���߽��ϴ�.");
            return;
        }

        // ��ư Ŭ�� �̺�Ʈ ����
        speedButton.onClick.AddListener(() => UpgradePlayer("speed"));
        attackButton.onClick.AddListener(() => UpgradePlayer("attack"));
        defenseButton.onClick.AddListener(() => UpgradePlayer("defense"));

        // �̼� Ÿ�� ����
        missionTarget = Instantiate(missionTargetPrefab);
        missionTarget.SetActive(false); // �̼� ���� ������ ��Ȱ��ȭ
    }

    private void Update()
    {
        // UI Ȱ��ȭ ���� ���콺 Ŀ�� ǥ��
        if (upgradeUI != null && upgradeUI.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // NPC�� ���� �ð� ���� ��ȣ�ۿ��� ������ ����
        if (!isMissionActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Debug.Log("Player�� ��ȣ�ۿ����� �ʾ� NPC�� �ı��˴ϴ�.");
                Destroy(gameObject);
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
                missionTarget.transform.position = randomPositions[Random.Range(0, randomPositions.Length)].position;
                missionTarget.SetActive(true);
                Debug.Log("�̼�: ������ ��ġ�� �̵��Ͽ� ������Ʈ�� �����ϱ�");

                yield return StartCoroutine(MissionTimer(10f, () =>
                    Vector3.Distance(player.transform.position, missionTarget.transform.position) < 1f
                ));
                missionTarget.SetActive(false);
                break;

            case MissionType.ReachMonster:
                GameObject monster = Instantiate(monsterPrefab, randomPositions[Random.Range(0, randomPositions.Length)].position, Quaternion.identity);
                monster.tag = "Monster"; // ���� �±� ����
                Debug.Log("�̼�: ��ȯ�� ���Ϳ� 12�� ���� �����ϱ�");

                if (monster.GetComponent<Collider>() == null)
                {
                    monster.AddComponent<BoxCollider>(); // Collider �߰�
                    Debug.Log("BoxCollider�� ���Ϳ� �߰��Ǿ����ϴ�.");
                }

                // MissionTimer ȣ��
                yield return StartCoroutine(MissionTimer(12f, () =>
                {
                    if (monster == null) return false; // ���Ͱ� �̹� �ı��� ���
                    float distance = Vector3.Distance(player.transform.position, monster.transform.position);
                    Debug.Log($"�÷��̾�� ������ �Ÿ�: {distance}");
                    return distance < 1f; // �Ÿ� ����
                }));

                // ���� ���� �� �ļ� ó��
                if (monster != null)
                {
                    Destroy(monster);
                    Debug.Log("���� ���� �Ϸ�");
                }

                // �̼� ���� �� ó��
                if (MissionSuccess())
                {
                    Debug.Log("�̼� ����: ��ȯ�� ���Ϳ� ���� ����");

                    // UI Ȱ��ȭ
                    GameObject panel = upgradeUI.transform.Find("Panel")?.gameObject;
                    if (panel != null)
                    {
                        panel.SetActive(true);
                        upgradeUI.SetActive(true); // UI Ȱ��ȭ
                        Debug.Log("���׷��̵� UI Ȱ��ȭ �Ϸ�");
                    }
                    else
                    {
                        Debug.LogError("Panel ������Ʈ�� ã�� �� �����ϴ�.");
                    }
                }
                else
                {
                    Debug.Log("�̼� ����!");
                }
                break;

            case MissionType.KillMonsters:
                Debug.Log("�̼�: 20�� ���� ��ȯ�� ���� 20���� óġ�ϱ�");
                for (int i = 0; i < 20; i++)
                {
                    Instantiate(monsterPrefab, randomPositions[Random.Range(0, randomPositions.Length)].position, Quaternion.identity);
                }

                yield return StartCoroutine(MissionTimer(20f, () =>
                    GameObject.FindGameObjectsWithTag("Monster").Length == 0
                ));
                break;
        }

        if (MissionSuccess())
        {
            Debug.Log("�̼� ����! ���� ���� UI�� ǥ���մϴ�.");

            GameObject panel = upgradeUI.transform.Find("Panel")?.gameObject;
            if (panel != null)
            {
                panel.SetActive(true);
                upgradeUI.SetActive(true);
            }
            else
            {
                Debug.LogError("Panel ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
    }

    private IEnumerator MissionTimer(float duration, System.Func<bool> successCondition)
    {
        float missionTimer = duration;
        while (missionTimer > 0f)
        {
            missionTimer -= Time.deltaTime;
            if (successCondition.Invoke())
            {
                yield break;
            }
            yield return null;
        }
    }

    private bool MissionSuccess()
    {
        switch (currentMission)
        {
            case MissionType.FindObject:
                return Vector3.Distance(player.transform.position, missionTarget.transform.position) < 1f;

            case MissionType.ReachMonster:
            case MissionType.KillMonsters:
                return GameObject.FindGameObjectsWithTag("Monster").Length == 0;

            default:
                return false;
        }
    }

    private void UpgradePlayer(string attribute)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        switch (attribute)
        {
            case "speed":
                playerStats.strength += 1; // �ӵ� ����
                Debug.Log("Player �ӵ��� �����߽��ϴ�.");
                break;
            case "attack":
                playerStats.strength += 1; // ���ݷ� ����
                Debug.Log("Player ���ݷ��� �����߽��ϴ�.");
                break;
            case "defense":
                playerStats.defense += 1; // ���� ����
                Debug.Log("Player ������ �����߽��ϴ�.");
                break;
        }
        upgradeUI.SetActive(false); // UI ��Ȱ��ȭ
    }
}
