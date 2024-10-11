using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : MonoBehaviour
{
    public static MonsterPoolManager instance;

    public GameObject monsterPrefab; // 몬스터 프리팹
    public int poolSize = 20; // 몬스터 최대 개수
    private Queue<GameObject> monsterPool = new Queue<GameObject>();

    private void Awake()
    {
        // 싱글톤 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 풀 초기화
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject monster = Instantiate(monsterPrefab);
            monster.SetActive(true); // 처음에는 비활성화 상태로
            monsterPool.Enqueue(monster);
        }
    }

    // 몬스터 가져오기
    public GameObject GetMonster()
    {
        if (monsterPool.Count > 0)
        {
            GameObject monster = monsterPool.Dequeue();
            monster.SetActive(true);
            return monster;
        }
        else
        {
            Debug.LogWarning("풀에 더 이상 몬스터가 없습니다!");
            return null;
        }
    }

    // 몬스터 반환하기
    public void ReturnMonster(GameObject monster)
    {
        monster.SetActive(false);
        monsterPool.Enqueue(monster);
    }
}
