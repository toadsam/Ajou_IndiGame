using UnityEngine;

public class Portal : MonoBehaviour
{
    public string portalName; // ��Ż �̸�

    private void OnTriggerEnter(Collider other)
    {
        // ��Ż�� ���� ��ü�� �÷��̾��� ���� ó��
        if (other.CompareTag("Player"))
        {
            // DialogueManager �̱����� ���� ��Ż ��ȭ UI ȣ��
            if (DialogueManager.instance != null)
            {
                DialogueManager.instance.ShowPortalDialogue(portalName); // ��Ż�� ��縦 ������
            }
            else
            {
                Debug.LogWarning("DialogueManager �ν��Ͻ��� �������� �ʽ��ϴ�.");
            }
        }
    }
}