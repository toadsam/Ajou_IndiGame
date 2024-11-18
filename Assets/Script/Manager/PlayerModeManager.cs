using System.Collections;
using UnityEngine;

public class PlayerModeManager : MonoBehaviour
{
    public PlayerController firstPersonController; // ���� �÷��̾� ��Ʈ�� ��ũ��Ʈ
    public PlayerTopDownController topDownController; // ž�ٿ� ��� ��ũ��Ʈ
    public TopDownCameraFollow topDownCameraFollow;
    public GameObject player; // �÷��̾� ������Ʈ ����
  //  public Transform topDownCameraPosition; // ž�ٿ� ����� �� ī�޶� ��ġ ����
    public Transform firstPersonCameraParent; // 1��Ī ����� �� ī�޶� �θ� ���� (�÷��̾��� Ư�� ��ġ, ��: �Ӹ�)
    public Rigidbody playerRigidbody;

    public Camera mainCamera; // ���� ī�޶� ����

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

    void Update()
    {
        // T Ű�� ������ �� ���� ��ȯ
        if (Input.GetKeyDown(KeyCode.T))
        {
            TogglePlayerMode();
        }
    }

    public void SetPlayerMode(PlayerMode mode)
    {
        currentMode = mode;

        switch (mode)
        {
            case PlayerMode.FirstPerson:
                firstPersonController.enabled = true;
                topDownController.enabled = false;
                UnlockYPosition();
                // ī�޶� �÷��̾��� Ư�� ��ġ�� �ڽ����� ����
                mainCamera.transform.SetParent(firstPersonCameraParent);
                mainCamera.transform.localPosition = Vector3.zero; // �θ��� ��ġ�� ����
                mainCamera.transform.localRotation = Quaternion.identity; // �θ��� ȸ���� ����
                topDownCameraFollow.enabled = false;

                break;

            case PlayerMode.TopDown:
                firstPersonController.enabled = false;
                topDownController.enabled = true;
                StartCoroutine(LockYPositionAfterDelay(3f));

                // ī�޶� �θ𿡼� �и��ϰ� ������ ž�ٿ� ��ġ�� ����
                mainCamera.transform.SetParent(null);
                // mainCamera.transform.position = topDownCameraPosition.position;
                //mainCamera.transform.rotation = topDownCameraPosition.rotation;
                topDownCameraFollow.enabled = true;

                break;
        }
    }

    // ������ ��ȯ�ϴ� �Լ�
    public void TogglePlayerMode()
    {
        if (currentMode == PlayerMode.FirstPerson)
        {
            SetPlayerMode(PlayerMode.TopDown);
        }
        else
        {
            SetPlayerMode(PlayerMode.FirstPerson);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PortalToTopDown"))
        {
            SetPlayerMode(PlayerMode.TopDown); // ��Ż�� ������ ž�ٿ� ���� ����
        }
        else if (other.CompareTag("PortalToFirstPerson"))
        {
            SetPlayerMode(PlayerMode.FirstPerson); // ��Ż�� ������ 1��Ī ���� ����
        }
    }

    private IEnumerator LockYPositionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        Debug.Log("Y���� �����Ǿ����ϴ�.");
    }

    private void UnlockYPosition()
    {
        playerRigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
        Debug.Log("Y�� ������ �����Ǿ����ϴ�.");
    }
}
