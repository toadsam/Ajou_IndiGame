using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpecialQuest : MonoBehaviour
{
    public GameObject player;                    // Player 오브젝트
    public GameObject missionTarget;             // 미션 타겟 오브젝트
    public GameObject monsterPrefab;             // 몬스터 프리팹
    public Transform[] randomPositions;          // 랜덤 위치 배열
    public GameObject upgradeUI;                 // 속성 선택 UI
    public Button speedButton, attackButton, defenseButton;

    private bool isMissionActive = false;
    private float timer = 30f;

    private enum MissionType { FindObject, ReachMonster, KillMonsters }
    private MissionType currentMission;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // 속성 선택 버튼 클릭 이벤트
        speedButton.onClick.AddListener(() => UpgradePlayer("speed"));
        attackButton.onClick.AddListener(() => UpgradePlayer("attack"));
        defenseButton.onClick.AddListener(() => UpgradePlayer("defense"));

        upgradeUI.SetActive(false); // 속성 선택 UI는 처음엔 비활성화
    }

    private void Update()
    {
        if (!isMissionActive)
        {
            timer -= Time.deltaTime;
            Debug.Log($"미션 대기 시간: {timer:F2}초 남음");

            if (timer <= 0f)
            {
                Debug.Log("Player와 상호작용하지 않아 NPC가 파괴됩니다.");
                Destroy(gameObject);  // 플레이어가 NPC와 상호작용하지 않으면 NPC 파괴
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isMissionActive && other.CompareTag("Player"))
        {
            Debug.Log("Player가 NPC와 접촉하여 미션을 시작합니다.");
            StartCoroutine(StartRandomMission());
            isMissionActive = true;
        }
    }

    private IEnumerator StartRandomMission()
    {
        currentMission = (MissionType)Random.Range(0, 3);
        Debug.Log($"랜덤 미션 시작: {currentMission}");

        switch (currentMission)
        {
            case MissionType.FindObject:
                Vector3 targetPosition = randomPositions[Random.Range(0, randomPositions.Length)].position;
                missionTarget.transform.position = targetPosition;
                missionTarget.SetActive(true);
                Debug.Log("미션: 지정된 위치로 이동하여 오브젝트와 접촉하기");

                yield return StartCoroutine(MissionTimer(10f, () => Vector3.Distance(player.transform.position, missionTarget.transform.position) < 1f));
                missionTarget.SetActive(false);
                break;

            case MissionType.ReachMonster:
                GameObject monster = Instantiate(monsterPrefab, randomPositions[Random.Range(0, randomPositions.Length)].position, Quaternion.identity);
                Debug.Log("미션: 소환된 몬스터와 12초 내에 접촉하기");

                yield return StartCoroutine(MissionTimer(12f, () => Vector3.Distance(player.transform.position, monster.transform.position) < 1f));
                Destroy(monster);
                break;

            case MissionType.KillMonsters:
                Debug.Log("미션: 20초 내에 소환된 몬스터 20마리 처치하기");
                for (int i = 0; i < 20; i++)
                {
                    Instantiate(monsterPrefab, randomPositions[Random.Range(0, randomPositions.Length)].position, Quaternion.identity);
                }

                yield return StartCoroutine(MissionTimer(20f, () => GameObject.FindGameObjectsWithTag("Monster").Length == 0));
                break;
        }

        if (MissionSuccess())
        {
            Debug.Log("미션 성공! 보상 선택 UI를 표시합니다.");
            upgradeUI.SetActive(true);
        }
        else
        {
            Debug.Log("미션 실패!");
        }
    }

    private IEnumerator MissionTimer(float duration, System.Func<bool> successCondition)
    {
        float missionTimer = duration;
        Debug.Log($"미션 타이머 시작: {duration}초");

        while (missionTimer > 0f)
        {
            missionTimer -= Time.deltaTime;
            Debug.Log($"미션 진행 중... 남은 시간: {missionTimer:F2}초");

            if (successCondition.Invoke())
            {
                Debug.Log("미션 성공 조건 충족");
                yield break;
            }

            yield return null;
        }
        Debug.Log("미션 시간 초과");
    }

    private bool MissionSuccess()
    {
        switch (currentMission)
        {
            case MissionType.FindObject:
                bool foundObject = Vector3.Distance(player.transform.position, missionTarget.transform.position) < 1f;
                Debug.Log(foundObject ? "오브젝트와 접촉 완료" : "오브젝트와 접촉 실패");
                return foundObject;

            case MissionType.ReachMonster:
                bool reachedMonster = GameObject.FindGameObjectsWithTag("Monster").Length == 0;
                Debug.Log(reachedMonster ? "몬스터 접촉 완료" : "몬스터 접촉 실패");
                return reachedMonster;

            case MissionType.KillMonsters:
                bool killedAllMonsters = GameObject.FindGameObjectsWithTag("Monster").Length == 0;
                Debug.Log(killedAllMonsters ? "모든 몬스터 처치 완료" : "모든 몬스터 처치 실패");
                return killedAllMonsters;

            default:
                return false;
        }
    }

    private void UpgradePlayer(string attribute)
    {
        Debug.Log($"{attribute} 속성을 선택했습니다.");

        switch (attribute)
        {
            case "speed":
                player.GetComponent<PlayerStats>().strength += 1; //일단은 힘으로 할게
                Debug.Log("Player 속도가 증가했습니다.");
                break;
            case "attack":
                player.GetComponent<PlayerStats>().strength += 1;
                Debug.Log("Player 공격력이 증가했습니다.");
                break;
            case "defense":
                player.GetComponent<PlayerStats>().defense += 1;
                Debug.Log("Player 방어력이 증가했습니다.");
                break;
        }

        upgradeUI.SetActive(false);
    }
}
