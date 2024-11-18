using System.Collections;
using UnityEngine;

public class PlayerItemManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static PlayerItemManager instance;

    private Vector3 originalSize;  // ���� ũ�� ����
    private float originalSpeed;   // ���� �ӵ� ����
    private int originalStrength;  // ���� �� ���� (PlayerStats���� ������)
    private Transform playerTransform;  // �÷��̾��� Transform
    private PlayerController playerController;  // �÷��̾� ��Ʈ�ѷ�
    private PlayerStats playerStats;  // �÷��̾� ���� ����

    private void Awake()
    {
        // �̱��� �ν��Ͻ��� �̹� �����ϴ��� Ȯ��
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // �� ��ȯ �ÿ��� ����
        }
        else
        {
            Destroy(gameObject);  // �ٸ� �ν��Ͻ��� ������ �ڽ��� �ı�
        }
    }

    private void Start()
    {
        playerTransform = transform;
        playerController = GetComponent<PlayerController>();
        playerStats = GetComponent<PlayerStats>();  // PlayerStats ��ũ��Ʈ ����

        // �÷��̾��� ���� ũ��, �ӵ� �� �� ����
        originalSize = playerTransform.localScale;
        originalSpeed = playerController.moveSpeed;
        originalStrength = playerStats.strength;  // PlayerStats���� �� �� ������
        //StartCoroutine(ChangeSize(5, 5, true));
    }

    public void StartSizeChange(float multiplier, float duration, bool increaseStrength = false) 
    {
        StartCoroutine(ChangeSize(multiplier, duration, increaseStrength));
    }
    public void StartSpeedChange(float multiplier, float duration)
    {
        StartCoroutine(ChangeSpeed(multiplier,duration));
    }
    public void StartSizeTiny(float multiplier, float duration, bool increaseStrength = false)
    {
        StartCoroutine(ChangeSize(multiplier, duration, increaseStrength));
    }

    public IEnumerator ChangeSize(float multiplier, float duration, bool increaseStrength = false)
    {
        // Transform �Ҵ��� Ȯ��
        if (playerTransform == null)
        {
            playerTransform = transform;
            Debug.Log("playerTransform was null, assigned transform.");
        }

        Vector3 targetSize = originalSize * multiplier;
        Vector3 startSize = playerTransform.localScale;

        Debug.Log($"Start Size: {startSize}, Target Size: {targetSize}");

        // �� ����
        if (increaseStrength)
        {
            playerStats.strength = originalStrength * 10;
            Debug.Log("Strength increased by 10x");
        }

        float elapsedTime = 0f;
        Debug.Log($"ChangeSize Coroutine Started. Duration: {duration}");

        // ũ�Ⱑ ���������� Ŀ������ �ϴ� ����
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;  // 0���� 1������ ����

            // ũ�� ����
            playerTransform.localScale = Vector3.Lerp(startSize, targetSize, t);
            Debug.Log($"Current Size (During Increase): {playerTransform.localScale}");

            // �� ������ ���
            yield return null;
        }

        // ���� ũ�� ����
        playerTransform.localScale = targetSize;
        Debug.Log("Final Size Set.");

        // ȿ���� ���� �� ���� ũ��� �ǵ���
        yield return new WaitForSeconds(duration);

        // ũ�� �����ϴ� ����
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            playerTransform.localScale = Vector3.Lerp(targetSize, originalSize, t);
            yield return null;
        }

        // ũ�� ���� �Ϸ� �� ���� ���·� ���ƿ�
        playerTransform.localScale = originalSize;
        playerStats.strength = originalStrength;
        Debug.Log("Player size and strength returned to normal.");
    }

    // �ӵ� ��ȭ �޼���
    public IEnumerator ChangeSpeed(float multiplier, float duration)
    {
        // �ӵ� ����
        playerController.moveSpeed = originalSpeed * multiplier;
        Debug.Log("Player speed increased!");

        yield return new WaitForSeconds(duration);

        // �ӵ� ���� ����
        playerController.moveSpeed = originalSpeed;
        Debug.Log("Player speed returned to normal.");
    }
}

