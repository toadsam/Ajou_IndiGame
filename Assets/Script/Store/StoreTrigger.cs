using UnityEngine;

public class StoreTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StoreManager.instance.ToggleStore(); // ���� ����/�ݱ�
        }
    }
}
