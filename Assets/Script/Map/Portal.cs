using UnityEngine;

public class Portal : MonoBehaviour
{
    public string portalName; // 포탈 이름

    private void OnTriggerEnter(Collider other)
    {
        // 포탈과 닿은 객체가 플레이어일 때만 처리
        if (other.CompareTag("Player"))
        {
            // DialogueManager 싱글톤을 통해 포탈 대화 UI 호출
            if (DialogueManager.instance != null)
            {
                DialogueManager.instance.ShowPortalDialogue(portalName); // 포탈의 대사를 보여줌
            }
            else
            {
                Debug.LogWarning("DialogueManager 인스턴스가 존재하지 않습니다.");
            }
        }
    }
}