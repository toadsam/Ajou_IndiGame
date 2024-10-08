using System.Collections;
using UnityEngine;

public class PlayerItemManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static PlayerItemManager instance;

    private Vector3 originalSize;  // 원래 크기 저장
    private float originalSpeed;   // 원래 속도 저장
    private int originalStrength;  // 원래 힘 저장 (PlayerStats에서 가져옴)
    private Transform playerTransform;  // 플레이어의 Transform
    private PlayerController playerController;  // 플레이어 컨트롤러
    private PlayerStats playerStats;  // 플레이어 스탯 관리

    private void Awake()
    {
        // 싱글톤 인스턴스가 이미 존재하는지 확인
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);  // 다른 인스턴스가 있으면 자신을 파괴
        }
    }

    private void Start()
    {
        playerTransform = transform;
        playerController = GetComponent<PlayerController>();
        playerStats = GetComponent<PlayerStats>();  // PlayerStats 스크립트 참조

        // 플레이어의 원래 크기, 속도 및 힘 저장
        originalSize = playerTransform.localScale;
        originalSpeed = playerController.moveSpeed;
        originalStrength = playerStats.strength;  // PlayerStats에서 힘 값 가져옴
    }

    public IEnumerator ChangeSize(float multiplier, float duration, bool increaseStrength = false)
    {
        // Transform 할당을 확인
        if (playerTransform == null)
        {
            playerTransform = transform;
            Debug.Log("playerTransform was null, assigned transform.");
        }

        Vector3 targetSize = originalSize * multiplier;
        Vector3 startSize = playerTransform.localScale;

        // 힘 증가
        if (increaseStrength)
        {
            playerStats.strength = originalStrength * 10;
            Debug.Log("Strength increased by 10x");
        }

        float elapsedTime = 0f;
        Debug.Log($"ChangeSize Coroutine Started. Duration: {duration}");

        // 크기가 점점 커지도록 루프
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;  // 진행 비율 (0에서 1로 변화)

            // 크기 변경
            playerTransform.localScale = Vector3.Lerp(startSize, targetSize, t);
            Debug.Log($"Current Size: {playerTransform.localScale}");

            yield return null;  // 다음 프레임까지 대기
        }

        // 최종 크기 설정
        playerTransform.localScale = targetSize;
        Debug.Log("Final Size Set.");

        // 효과가 끝난 후 원래 크기로 되돌림
        yield return new WaitForSeconds(duration);

        // 원래 크기 복원 루프
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            playerTransform.localScale = Vector3.Lerp(targetSize, originalSize, t);
            yield return null;
        }

        playerTransform.localScale = originalSize;
        playerStats.strength = originalStrength;
        Debug.Log("Player size and strength returned to normal.");
    }



// 속도 변화 메서드
public IEnumerator ChangeSpeed(float multiplier, float duration)
    {
        // 속도 증가
        playerController.moveSpeed = originalSpeed * multiplier;
        Debug.Log("Player speed increased!");

        yield return new WaitForSeconds(duration);

        // 속도 원상 복구
        playerController.moveSpeed = originalSpeed;
        Debug.Log("Player speed returned to normal.");
    }
}

