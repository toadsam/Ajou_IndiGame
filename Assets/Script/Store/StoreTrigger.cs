using UnityEngine;

public class StoreTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StoreManager.instance.ToggleStore(); // 상점 열기/닫기
        }
    }
}
