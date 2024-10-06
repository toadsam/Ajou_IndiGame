using UnityEngine;

public class PlayerModeManager : MonoBehaviour
{
    public PlayerController firstPersonController; // 기존 플레이어 컨트롤 스크립트
    public PlayerTopDownController topDownController; // 탑다운 모드 스크립트
    public TopDownCameraFollow topDownCameraFollow;
    public GameObject player; // 플레이어 오브젝트 참조
  //  public Transform topDownCameraPosition; // 탑다운 모드일 때 카메라 위치 지정
    public Transform firstPersonCameraParent; // 1인칭 모드일 때 카메라 부모 지정 (플레이어의 특정 위치, 예: 머리)

    public Camera mainCamera; // 메인 카메라 참조

    public enum PlayerMode { FirstPerson, TopDown }
    public PlayerMode currentMode;

    private void Awake()
    {
        // 씬 전환 시에도 플레이어 오브젝트가 사라지지 않도록 설정
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
        // T 키를 눌렀을 때 시점 전환
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

                // 카메라를 플레이어의 특정 위치의 자식으로 설정
                mainCamera.transform.SetParent(firstPersonCameraParent);
                mainCamera.transform.localPosition = Vector3.zero; // 부모의 위치에 맞춤
                mainCamera.transform.localRotation = Quaternion.identity; // 부모의 회전에 맞춤
                topDownCameraFollow.enabled = false;

                break;

            case PlayerMode.TopDown:
                firstPersonController.enabled = false;
                topDownController.enabled = true;

                // 카메라를 부모에서 분리하고 지정된 탑다운 위치로 설정
                mainCamera.transform.SetParent(null);
                // mainCamera.transform.position = topDownCameraPosition.position;
                //mainCamera.transform.rotation = topDownCameraPosition.rotation;
                topDownCameraFollow.enabled = true;

                break;
        }
    }

    // 시점을 전환하는 함수
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
            SetPlayerMode(PlayerMode.TopDown); // 포탈에 닿으면 탑다운 모드로 변경
        }
        else if (other.CompareTag("PortalToFirstPerson"))
        {
            SetPlayerMode(PlayerMode.FirstPerson); // 포탈에 닿으면 1인칭 모드로 변경
        }
    }
}
