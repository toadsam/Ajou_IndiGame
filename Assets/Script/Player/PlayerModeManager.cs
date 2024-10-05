using UnityEngine;

public class PlayerModeManager : MonoBehaviour
{
    public PlayerController firstPersonController; // 기존 플레이어 컨트롤 스크립트
    public PlayerTopDownController topDownController; // 집중포화 모드 스크립트
    public GameObject player; // 플레이어 오브젝트 참조

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
            SetPlayerMode(PlayerMode.TopDown); // 포탈에 닿으면 집중포화 모드로 변경
        }
        else if (other.CompareTag("PortalToFirstPerson"))
        {
            SetPlayerMode(PlayerMode.FirstPerson); // 포탈에 닿으면 원래 모드로 변경
        }
    }
}
