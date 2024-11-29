using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialQuest : MonoBehaviour
{
    public GameObject missionTargetPrefab; // �̼� Ÿ�� ������
    public GameObject monsterPrefab;      // ���� ������
    public GameObject playTimeInfo;       // ���ѽð� UI
    public GameObject playStageTitle;     // �̼� �̸� �� ���� ǥ�� UI

    [Header("Text References")]
    public TextMeshProUGUI timeText;      // ���ѽð� �ؽ�Ʈ (playTimeInfo�� �ڽ�)
    public TextMeshProUGUI stageTitleText; // �̼� �̸� �ؽ�Ʈ (playStageTitle�� �ڽ�)

    private GameObject player;            // Player ������Ʈ
    private GameObject missionTarget;     // �̼� Ÿ�� �ν��Ͻ�
    private List<GameObject> spawnedMonsters = new List<GameObject>(); // ������ ���� ����Ʈ

    private bool isMissionActive = false;
    private float timer = 30f;

    public static bool isSpecial;

    private enum MissionType { FindObject, ReachMonster, KillMonsters }
    private MissionType currentMission;

    private void Start()
    {
        isSpecial = false;

        // �÷��̾� �ʱ�ȭ
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        // UI �ʱ�ȭ
        if (playTimeInfo != null) playTimeInfo.SetActive(false);
        if (playStageTitle != null) playStageTitle.SetActive(false);

        missionTarget = Instantiate(missionTargetPrefab);
        missionTarget.SetActive(false);

        // ����Ʈ ���� �˸�
        StartCoroutine(ShowQuestAnnouncement());
    }

    private void Update()
    {
        // NPC�� ��ȣ�ۿ����� ������ Ÿ�̸Ӱ� ����
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
            StartCoroutine(StartRandomMission());
            isMissionActive = true;
        }
    }

    private IEnumerator StartRandomMission()
    {
        currentMission = MissionType.KillMonsters;

        // �̼� �̸� UI ǥ��
        if (stageTitleText != null)
        {
            stageTitleText.text = $"���߹̼�! {GetMissionText(currentMission)}";
        }
        if (playStageTitle != null)
        {
            playStageTitle.SetActive(true);
            yield return new WaitForSeconds(3f);
            playStageTitle.SetActive(false);
        }

        // ���� ����
        Debug.Log("�̼�: 20�� ���� ���� 5���� óġ�ϱ�");
        for (int i = 0; i < 100; i++)
        {
            var monster = Instantiate(monsterPrefab, RandomPosition(), Quaternion.identity);
            spawnedMonsters.Add(monster);
        }

        // ���� �ð� UI Ȱ��ȭ
        if (playTimeInfo != null) playTimeInfo.SetActive(true);

        // Ÿ�̸� ����
        bool success = false;
        yield return StartCoroutine(MissionTimer(20f, () =>
        {
            success = spawnedMonsters.TrueForAll(m => m == null); // ��� ���Ͱ� �ı��Ǿ����� Ȯ��
            return success;
        }));

        // ���� �ð� UI ��Ȱ��ȭ
        if (playTimeInfo != null) playTimeInfo.SetActive(false);

        // �̼� ��� UI ǥ��
        if (playStageTitle != null && stageTitleText != null)
        {
            playStageTitle.SetActive(true);
            if (success)
            {
                stageTitleText.text = "�̼� ����!";
            }
            else
            {
                stageTitleText.text = "�̼� ����!";
            }

            yield return new WaitForSeconds(3f);
            playStageTitle.SetActive(false);
        }

        // ��� ���� ����
        ClearSpawnedMonsters();

        // �� ��ũ��Ʈ ����
        Destroy(gameObject);
    }


    private IEnumerator MissionTimer(float duration, System.Func<bool> successCondition)
    {
        float missionTimer = duration;
        while (missionTimer > 0f)
        {
            missionTimer -= Time.deltaTime;

            // ���� �ð� UI ������Ʈ
            if (timeText != null)
            {
                timeText.text = $"���� �ð�: {Mathf.CeilToInt(missionTimer)}��";
            }

            // ���� ���� ���� �� ����
            if (successCondition.Invoke())
            {
                yield break;
            }

            yield return null;
        }
    }

    private void ClearSpawnedMonsters()
    {
        for (int i = spawnedMonsters.Count - 1; i >= 0; i--)
        {
            if (spawnedMonsters[i] != null)
            {
                Destroy(spawnedMonsters[i]);
            }
        }
        spawnedMonsters.Clear();
    }
    private string GetMissionText(MissionType missionType)
    {
        switch (missionType)
        {
            case MissionType.FindObject:
                return "������ ��ġ���� ��ü�� ã������!";
            case MissionType.ReachMonster:
                return "��ȯ�� ���Ϳ� �����ϼ���!";
            case MissionType.KillMonsters:
                return "���� 100������ óġ�ϼ���!";
            default:
                return "�� �� ���� �̼��Դϴ�.";
        }
    }

    private IEnumerator ShowQuestAnnouncement()
    {
        if (stageTitleText != null)
        {
            stageTitleText.text = "����� ����Ʈ ����!";
        }
        if (playStageTitle != null)
        {
            playStageTitle.SetActive(true);
            yield return new WaitForSeconds(3f);
            playStageTitle.SetActive(false);
        }
    }

    private Vector3 RandomPosition()
    {
        Transform randomPositionParent = GameObject.FindGameObjectWithTag("RandomPosition").transform;
        if (randomPositionParent == null || randomPositionParent.childCount == 0)
            return Vector3.zero;

        Transform randomPosition = randomPositionParent.GetChild(Random.Range(0, randomPositionParent.childCount));
        return randomPosition.position;
    }
}
