using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialQuest : MonoBehaviour
{
    public GameObject missionTargetPrefab; // 미션 타겟 프리팹
    public GameObject monsterPrefab;      // 몬스터 프리팹
    public GameObject playTimeInfo;       // 제한시간 UI
    public GameObject playStageTitle;     // 미션 이름 및 상태 표시 UI

    [Header("Text References")]
    public TextMeshProUGUI timeText;      // 제한시간 텍스트 (playTimeInfo의 자식)
    public TextMeshProUGUI stageTitleText; // 미션 이름 텍스트 (playStageTitle의 자식)

    private GameObject player;            // Player 오브젝트
    private GameObject missionTarget;     // 미션 타겟 인스턴스
    private List<GameObject> spawnedMonsters = new List<GameObject>(); // 생성된 몬스터 리스트

    private bool isMissionActive = false;
    private float timer = 30f;

    public static bool isSpecial;

    private enum MissionType { FindObject, ReachMonster, KillMonsters }
    private MissionType currentMission;

    private void Start()
    {
        isSpecial = false;

        // 플레이어 초기화
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player 오브젝트를 찾을 수 없습니다.");
            return;
        }

        // UI 초기화
        if (playTimeInfo != null) playTimeInfo.SetActive(false);
        if (playStageTitle != null) playStageTitle.SetActive(false);

        missionTarget = Instantiate(missionTargetPrefab);
        missionTarget.SetActive(false);

        // 퀘스트 시작 알림
        StartCoroutine(ShowQuestAnnouncement());
    }

    private void Update()
    {
        // NPC와 상호작용하지 않으면 타이머가 감소
        if (!isMissionActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Debug.Log("Player와 상호작용하지 않아 NPC가 파괴됩니다.");
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

        // 미션 이름 UI 표시
        if (stageTitleText != null)
        {
            stageTitleText.text = $"돌발미션! {GetMissionText(currentMission)}";
        }
        if (playStageTitle != null)
        {
            playStageTitle.SetActive(true);
            yield return new WaitForSeconds(3f);
            playStageTitle.SetActive(false);
        }

        // 몬스터 생성
        Debug.Log("미션: 20초 내에 몬스터 5마리 처치하기");
        for (int i = 0; i < 100; i++)
        {
            var monster = Instantiate(monsterPrefab, RandomPosition(), Quaternion.identity);
            spawnedMonsters.Add(monster);
        }

        // 제한 시간 UI 활성화
        if (playTimeInfo != null) playTimeInfo.SetActive(true);

        // 타이머 시작
        bool success = false;
        yield return StartCoroutine(MissionTimer(20f, () =>
        {
            success = spawnedMonsters.TrueForAll(m => m == null); // 모든 몬스터가 파괴되었는지 확인
            return success;
        }));

        // 제한 시간 UI 비활성화
        if (playTimeInfo != null) playTimeInfo.SetActive(false);

        // 미션 결과 UI 표시
        if (playStageTitle != null && stageTitleText != null)
        {
            playStageTitle.SetActive(true);
            if (success)
            {
                stageTitleText.text = "미션 성공!";
            }
            else
            {
                stageTitleText.text = "미션 실패!";
            }

            yield return new WaitForSeconds(3f);
            playStageTitle.SetActive(false);
        }

        // 모든 몬스터 제거
        ClearSpawnedMonsters();

        // 이 스크립트 제거
        Destroy(gameObject);
    }


    private IEnumerator MissionTimer(float duration, System.Func<bool> successCondition)
    {
        float missionTimer = duration;
        while (missionTimer > 0f)
        {
            missionTimer -= Time.deltaTime;

            // 제한 시간 UI 업데이트
            if (timeText != null)
            {
                timeText.text = $"남은 시간: {Mathf.CeilToInt(missionTimer)}초";
            }

            // 성공 조건 충족 시 종료
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
                return "지정된 위치에서 물체를 찾으세요!";
            case MissionType.ReachMonster:
                return "소환된 몬스터와 접촉하세요!";
            case MissionType.KillMonsters:
                return "몬스터 100마리를 처치하세요!";
            default:
                return "알 수 없는 미션입니다.";
        }
    }

    private IEnumerator ShowQuestAnnouncement()
    {
        if (stageTitleText != null)
        {
            stageTitleText.text = "스페셜 퀘스트 등장!";
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
