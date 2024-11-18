using System.Collections;
using UnityEngine;

public class RobotSummoner : MonoBehaviour
{
    public GameObject robotPrefab; // 로봇 프리팹
    public Transform playerTransform; // 플레이어 위치 참조
    public float summonDuration = 20f; // 소환된 로봇의 지속 시간
    public float cooldownTime = 20f; // 스킬 재사용 대기 시간

    private bool isOnCooldown = false; // 스킬 대기 상태 확인

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isOnCooldown)
        {
            StartCoroutine(SummonRobot()); // Q 버튼으로 로봇 소환
        }
    }

    private IEnumerator SummonRobot()
    {
        isOnCooldown = true;

        // 플레이어 앞쪽에 로봇 생성
        Vector3 summonPosition = playerTransform.position + playerTransform.forward * 2f;
        GameObject robot = Instantiate(robotPrefab, summonPosition, Quaternion.identity);

        Debug.Log("로봇 소환됨"); // 로봇 소환 확인

        // 로봇이 summonDuration(기본값 10초) 동안 유지된 후 사라짐
        yield return new WaitForSeconds(summonDuration);

        Debug.Log("로봇 사라짐"); // 로봇 사라짐 확인
        Destroy(robot);

        // 쿨타임 적용
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;

        Debug.Log("스킬 쿨타임 종료"); // 쿨타임 종료 확인
    }
}
