using UnityEngine;

public class PlayerModeManager : MonoBehaviour
{
    public PlayerController firstPersonController; // ���� �÷��̾� ��Ʈ�� ��ũ��Ʈ
    public PlayerTopDownController topDownController; // ������ȭ ��� ��ũ��Ʈ
    public GameObject player; // �÷��̾� ������Ʈ ����

    public enum PlayerMode { FirstPerson, TopDown }
    public PlayerMode currentMode;

    private void Awake()
    {
        // �� ��ȯ �ÿ��� �÷��̾� ������Ʈ�� ������� �ʵ��� ����
        if (player != null)
        {
            DontDestroyOnLoad(player);
        }
    }

    void Start()
    {
        SetPlayerMode(currentMode);
    }

    public void SetPlayerMode(PlayerMode mode)
    {
        currentMode = mode;

        switch (mode)
        {
            case PlayerMode.FirstPerson:
                firstPersonController.enabled = true;
                topDownController.enabled = false;
                break;
            case PlayerMode.TopDown:
                firstPersonController.enabled = false;
                topDownController.enabled = true;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PortalToTopDown"))
        {
            SetPlayerMode(PlayerMode.TopDown); // ��Ż�� ������ ������ȭ ���� ����
        }
        else if (other.CompareTag("PortalToFirstPerson"))
        {
            SetPlayerMode(PlayerMode.FirstPerson); // ��Ż�� ������ ���� ���� ����
        }
    }
}
