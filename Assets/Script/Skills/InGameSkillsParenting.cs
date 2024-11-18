using UnityEngine;

public class InGameSkillsParenting : MonoBehaviour
{
    public GameObject inGameSkills; // InGameSkills 오브젝트 참조

    private void Start()
    {
        // Player 오브젝트를 태그로 찾음
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            // Player 오브젝트 안에서 "Chito1" 자식을 찾음
            Transform chito1Transform = player.transform.Find("Chito");

            if (inGameSkills != null && chito1Transform != null)
            {
                // InGameSkills를 Chito1의 자식으로 설정하고 로컬 좌표를 (0, 0, 0)으로 초기화
                inGameSkills.transform.SetParent(chito1Transform, false);
                inGameSkills.transform.localPosition = Vector3.zero;
                Debug.Log("InGameSkills가 Chito1의 자식이 되었고 위치가 Chito1 기준으로 초기화되었습니다.");
            }
            else
            {
                Debug.LogWarning("InGameSkills 또는 Chito1 오브젝트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("Player 오브젝트를 찾을 수 없습니다.");
        }
    }
}
