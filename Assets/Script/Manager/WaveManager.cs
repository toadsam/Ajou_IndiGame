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
    public GameObject bossWavePanel; // ���� ���̺� �г�

    private int currentWave = 1;
    private int monstersRemaining;
    private Queue<GameObject> monsterPool = new Queue<GameObject>();

    public GameObject specialNPCPrefab; // SpecialNPC ������

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

        StartWave(1); // ù ��° ���̺� ����
    }

    private void Update()
    {
        // Ȱ��ȭ�� ���� �� üũ
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
                Debug.Log("ù ��° ���̺� ���� - ���� ����: " + monstersRemaining);
                break;
            case 2:
                SpawnSpecialNPC();
                monstersRemaining = 10;
                SpawnMonsters(secondWaveMonsterPrefab, monstersRemaining);
                Debug.Log("�� ��° ���̺� ���� - ���� ����: " + monstersRemaining);
                break;
            case 3:
                SpawnSpecialNPC();
                monstersRemaining = 1;
                SpawnMonsters(thirdWaveMonsterPrefab, monstersRemaining);
                Debug.Log("�� ��° ���̺� ���� - ���� ����: " + monstersRemaining);
                break;
            case 4:
                SpawnSpecialNPC();
                monstersRemaining = 1;
                SpawnMonsters(fourthWaveMonsterPrefab, monstersRemaining);
                Debug.Log("�� ��° ���̺� ���� - ���� ����: " + monstersRemaining);
                break;
            case 5:
                StartCoroutine(ShowBossWavePanel());
                monstersRemaining = 1;
                SpawnMonsters(bossPrefab, monstersRemaining);
                Debug.Log("���� ���̺� ���� - ���� ����: " + monstersRemaining);
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
        Debug.Log("SpecialNPC�� ��ȯ�Ǿ����ϴ�.");
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

        // ���Ͱ� ��� ������� �� ���� ���̺�� �̵�
        if (activeMonsters == 0 && monstersRemaining > 0)
        {
            MonsterDefeated();
        }
    }

    public void MonsterDefeated()
    {
        monstersRemaining--;

        Debug.Log($"���Ͱ� óġ�Ǿ����ϴ�. ���� ���� ��: {monstersRemaining}");

        if (monstersRemaining <= 0)
        {
            currentWave++;
            Debug.Log($"��� ���� óġ, ���� ���̺�� �Ѿ�ϴ�. ���� ���̺�: {currentWave}");

            if (currentWave <= 5)
            {
                StartWave(currentWave);
            }
            else
            {
                Debug.Log("��� ���̺� �Ϸ�!");
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
            Debug.LogWarning("bossWavePanel�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
