using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    private static CanvasManager instance;

    private void Awake()
    {
        // 캔버스가 씬 전환 시 파괴되지 않도록 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 이 오브젝트를 파괴하지 않음
        }
        else
        {
            Destroy(gameObject); // 이미 캔버스가 있을 경우 새로운 것은 파괴
        }
    }
}
