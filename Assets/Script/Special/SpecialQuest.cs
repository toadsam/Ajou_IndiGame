using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpecialQuest : MonoBehaviour
{
    public GameObject missionTargetPrefab;        // 미션 타겟 프리팹
    public GameObject monsterPrefab;              // 몬스터 프리팹

    private GameObject player;                    // Player 오브젝트
    private GameObject missionTarget;             // 미션 타겟 인스턴스
    private GameObject upgradeUI;                 // 속성 선택 UI 인스턴스
    private Button speedButton, attackButton, defenseButton;
    private Transform[] randomPositions;          // 랜덤 위치 배열

    private bool isMissionActive = false;
    private float timer = 30f;

    private enum MissionType { FindObject, ReachMonster, KillMonsters }
    private MissionType currentMission;

    private void Start()
    {
        // 태그로 오브젝트 찾기
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player 오브젝트를 찾을 수 없습니다.");
            return;
        }

        // randomPositions을 특정 태그로 할당
        GameObject randomPositionParent = GameObject.FindGameObjectWithTag("RandomPosition");
        if (randomPositionParent != null)
        {
            randomPositions = new Transform[randomPositionParent.transform.childCount];
            for (int i = 0; i < randomPositionParent.transform.childCount; i++)
            {
                randomPositions[i] = randomPositionParent.transform.GetChild(i);
            }
            Debug.Log("Random Positions 배열 할당 완료");
        }
        else
        {
            Debug.LogError("RandomPosition 태그의 오브젝트가 없습니다.");
            return;
        }

        // upgradeUI를 태그로 찾기
        upgradeUI = GameObject.FindGameObjectWithTag("UpgradeUI");
        if (upgradeUI == null)
        {
            Debug.LogError("UpgradeUI 오브젝트를 찾을 수 없습니다.");
            return;
        }

        upgradeUI.SetActive(true); // 임시 활성화

        // Panel 내부에서 버튼 찾기
        Transform panel = upgradeUI.transform.Find("Panel");
        if (panel == null)
        {
            Debug.LogError("Panel 오브젝트를 찾을 수 없습니다.");
            return;
        }

        speedButton = panel.Find("SpeedButton")?.GetComponent<Button>();
        attackButton = panel.Find("AttackButton")?.GetComponent<Button>();
        defenseButton = panel.Find("DefenseButton")?.GetComponent<Button>();

        // 각 버튼이 제대로 할당되었는지 확인하는 디버그 메시지
        Debug.Log($"Speed Button 할당: {speedButton != null}");
        Debug.Log($"Attack Button 할당: {attackButton != null}");
        Debug.Log($"Defense Button 할당: {defenseButton != null}");

        upgradeUI.SetActive(false); // 다시 비활성화

        if (speedButton == null || attackButton == null || defenseButton == null)
        {
            Debug.LogError("버튼 중 하나를 찾지 못했습니다.");
            return;
        }

        // 버튼 클릭 이벤트 설정
        speedButton.onClick.AddListener(() => UpgradePlayer("speed"));
        attackButton.onClick.AddListener(() => UpgradePlayer("attack"));
        defenseButton.onClick.AddListener(() => UpgradePlayer("defense"));

        // 미션 타겟 생성
        missionTarget = Instantiate(missionTargetPrefab);
        missionTarget.SetActive(false); // 미션 시작 전까지 비활성화
    }

    private void Update()
    {
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
                missionTarget.transform.position = randomPositions[Random.Range(0, randomPositions.Length)].position;
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
                playerStats.strength += 1;
                Debug.Log("Player 속도가 증가했습니다.");
                break;
            case "attack":
                playerStats.strength += 1;
                Debug.Log("Player 공격력이 증가했습니다.");
                break;
            case "defense":
                playerStats.defense += 1;
                Debug.Log("Player 방어력이 증가했습니다.");
                break;
        }
        upgradeUI.SetActive(false);
    }
}
