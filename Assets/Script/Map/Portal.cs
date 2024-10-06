using UnityEngine;

public class Portal : MonoBehaviour
{
    public string portalName; // ��Ż �̸��� �����Ͽ� �̵��� �� �̸��� ����

    private void OnTriggerEnter(Collider other)
    {
        // ��Ż�� ���� ��ü�� �÷��̾��� ���� ó��
        if (other.CompareTag("Player"))
        {
            // SceneLoader �̱����� ���� �� �ε� ��û
            if (SceneLoader.instance != null)
            {
                SceneLoader.instance.LoadSceneBasedOnPortalName(portalName);
            }
            else
            {
                Debug.LogWarning("SceneLoader �ν��Ͻ��� �������� �ʽ��ϴ�.");
            }
        }
    }
}
