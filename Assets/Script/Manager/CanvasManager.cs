using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    private static CanvasManager instance;

    private void Awake()
    {
        // ĵ������ �� ��ȯ �� �ı����� �ʵ��� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ������Ʈ�� �ı����� ����
        }
        else
        {
            Destroy(gameObject); // �̹� ĵ������ ���� ��� ���ο� ���� �ı�
        }
    }
}
