using UnityEngine;
using UnityEngine.SceneManagement; // �� ��ȯ�� ���� �ʿ�

public class Portal : MonoBehaviour
{
    // ��Ż �̸��� ���� �̵��� �� �̸��� �����ϴ� ����
    public string portalName; // �� ��Ż�� ������ �̸����� ���� ����

    // �÷��̾ ��Ż�� ����� �� ȣ��Ǵ� �޼ҵ�
    private void OnTriggerEnter(Collider other)
    {
        // ��Ż�� ���� ��ü�� �÷��̾��� ���� ó��
        if (other.CompareTag("Player"))
        {
            LoadSceneBasedOnPortalName();
        }
    }

    // ��Ż �̸��� ���� ���� �ε��ϴ� �Լ�
    private void LoadSceneBasedOnPortalName()
    {
        switch (portalName)
        {
            case "SeonghoHallPortal":
                SceneManager.LoadScene("SeonghoHallScene"); // ��ȣ�� ������ �̵�
                break;
            case "WoncheonHallPortal":
                SceneManager.LoadScene("WoncheonHallScene"); // ��õ�� ������ �̵�
                break;
            case "PalDalHallPortal":
                SceneManager.LoadScene("PalDalHallScene"); // �ȴް� ������ �̵�
                break;
            case "SanhakHallPortal":
                SceneManager.LoadScene("SanhakHallScene"); // ���п� ������ �̵�
                break;
            default:
                Debug.LogWarning("��Ż �̸��� �߸��Ǿ����ϴ�: " + portalName);
                break;
        }
    }
}
