using UnityEngine;

public class InGameSkillsParenting : MonoBehaviour
{
    public GameObject inGameSkills; // InGameSkills ������Ʈ ����

    private void Start()
    {
        // Player ������Ʈ�� �±׷� ã��
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            // Player ������Ʈ �ȿ��� "Chito1" �ڽ��� ã��
            Transform chito1Transform = player.transform.Find("Chito");

            if (inGameSkills != null && chito1Transform != null)
            {
                // InGameSkills�� Chito1�� �ڽ����� �����ϰ� ���� ��ǥ�� (0, 0, 0)���� �ʱ�ȭ
                inGameSkills.transform.SetParent(chito1Transform, false);
                inGameSkills.transform.localPosition = Vector3.zero;
                Debug.Log("InGameSkills�� Chito1�� �ڽ��� �Ǿ��� ��ġ�� Chito1 �������� �ʱ�ȭ�Ǿ����ϴ�.");
            }
            else
            {
                Debug.LogWarning("InGameSkills �Ǵ� Chito1 ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("Player ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }
}
