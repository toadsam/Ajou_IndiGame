using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : MonoBehaviour
{
    public static MonsterPoolManager instance;

    public GameObject monsterPrefab; // ���� ������
    public int poolSize = 20; // ���� �ִ� ����
    private Queue<GameObject> monsterPool = new Queue<GameObject>();

    private void Awake()
    {
        // �̱��� ����
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

    // Ǯ �ʱ�ȭ
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject monster = Instantiate(monsterPrefab);
            monster.SetActive(true); // ó������ ��Ȱ��ȭ ���·�
            monsterPool.Enqueue(monster);
        }
    }

    // ���� ��������
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
            Debug.LogWarning("Ǯ�� �� �̻� ���Ͱ� �����ϴ�!");
            return null;
        }
    }

    // ���� ��ȯ�ϱ�
    public void ReturnMonster(GameObject monster)
    {
        monster.SetActive(false);
        monsterPool.Enqueue(monster);
    }
}
