using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public GameObject firstWaveMonsterPrefab;
    public GameObject secondWaveMonsterPrefab;
    public GameObject thirdWaveMonsterPrefab;
    public GameObject fourthWaveMonsterPrefab;
    public GameObject bossPrefab;
    public Transform[] respawnPoints;
    public GameObject bossWavePanel; // 보스 웨이브 패널

    private int currentWave = 1;
    private int monstersRemaining;
    private Queue<GameObject> monsterPool = new Queue<GameObject>();

    public GameObject specialNPCPrefab; // SpecialNPC 프리팹

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Initialize(firstWaveMonsterPrefab, secondWaveMonsterPrefab, bossPrefab, respawnPoints);
    }

    public void Initialize(GameObject firstMonster, GameObject secondMonster, GameObject boss, Transform[] respawns)
    {
        firstWaveMonsterPrefab = firstMonster;
        secondWaveMonsterPrefab = secondMonster;
        bossPrefab = boss;
        respawnPoints = respawns;

        StartWave(1); // 첫 번째 웨이브 시작
    }

    private void Update()
    {
        // 활성화된 몬스터 수 체크
        CheckMonstersRemaining();
    }

    private void StartWave(int wave)
    {
        currentWave = wave;

        switch (currentWave)
        {
            case 1:
                monstersRemaining = 1;
                SpawnMonsters(firstWaveMonsterPrefab, monstersRemaining);
                Debug.Log("첫 번째 웨이브 시작 - 남은 몬스터: " + monstersRemaining);
                break;
            case 2:
                SpawnSpecialNPC();
                monstersRemaining = 10;
                SpawnMonsters(secondWaveMonsterPrefab, monstersRemaining);
                Debug.Log("두 번째 웨이브 시작 - 남은 몬스터: " + monstersRemaining);
                break;
            case 3:
                SpawnSpecialNPC();
                monstersRemaining = 1;
                SpawnMonsters(thirdWaveMonsterPrefab, monstersRemaining);
                Debug.Log("세 번째 웨이브 시작 - 남은 몬스터: " + monstersRemaining);
                break;
            case 4:
                SpawnSpecialNPC();
                monstersRemaining = 1;
                SpawnMonsters(fourthWaveMonsterPrefab, monstersRemaining);
                Debug.Log("네 번째 웨이브 시작 - 남은 몬스터: " + monstersRemaining);
                break;
            case 5:
                StartCoroutine(ShowBossWavePanel());
                monstersRemaining = 1;
                SpawnMonsters(bossPrefab, monstersRemaining);
                Debug.Log("보스 웨이브 시작 - 남은 몬스터: " + monstersRemaining);
                break;
        }
    }

    private void SpawnMonsters(GameObject monsterPrefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var spawnPoint = respawnPoints[Random.Range(0, respawnPoints.Length)];
            GameObject monster = Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity);
            monsterPool.Enqueue(monster);
        }
    }


    private void SpawnSpecialNPC()
    {
        Transform spawnPoint = respawnPoints[Random.Range(0, respawnPoints.Length)];
        Instantiate(specialNPCPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("SpecialNPC가 소환되었습니다.");
    }

    private void CheckMonstersRemaining()
    {
        int activeMonsters = 0;
        foreach (var monster in monsterPool)
        {
            if (monster != null && monster.activeInHierarchy)
            {
                activeMonsters++;
            }
        }

        // 몬스터가 모두 사라졌을 때 다음 웨이브로 이동
        if (activeMonsters == 0 && monstersRemaining > 0)
        {
            MonsterDefeated();
        }
    }

    public void MonsterDefeated()
    {
        monstersRemaining--;

        Debug.Log($"몬스터가 처치되었습니다. 남은 몬스터 수: {monstersRemaining}");

        if (monstersRemaining <= 0)
        {
            currentWave++;
            Debug.Log($"모든 몬스터 처치, 다음 웨이브로 넘어갑니다. 현재 웨이브: {currentWave}");

            if (currentWave <= 5)
            {
                StartWave(currentWave);
            }
            else
            {
                Debug.Log("모든 웨이브 완료!");
            }
        }
    }


    private IEnumerator ShowBossWavePanel()
    {
        if (bossWavePanel != null)
        {
            bossWavePanel.SetActive(true);
            yield return new WaitForSeconds(3f);
            bossWavePanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("bossWavePanel이 할당되지 않았습니다.");
        }
    }
}
