using UnityEngine;

public class Item : MonoBehaviour

{
    // �������� ������ �����ϴ� enum
    public enum ItemType
    {
        Giant,  // ���̾�Ʈ ������ (50�� Ŀ��)
        Speed,   // ���ǵ� ������ (10�� ������)
        Tiny

    }

    public ItemType itemType; // ������ Ÿ�� ����

    public float giantSizeMultiplier;  // ���̾�Ʈ ������ ũ�� ����
    public float tinySizeMultiplier;  // �۾����� ������ ũ�� ����
    public float speedMultiplier;      // ���ǵ� ������ �ӵ� ����
    public float effectDuration;       // ȿ�� ���� �ð�

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // �÷��̾�� �浹�ϸ�
        {
            switch (itemType)
            {
                case ItemType.Giant:
                    Debug.Log("1");
                    // ũ�� 50��� ���� + �� 10��� ����
                    PlayerItemManager.instance.StartSizeChange(giantSizeMultiplier,effectDuration,true);
                    break;
                case ItemType.Speed:
                    // �ӵ� 10��� ����
                    PlayerItemManager.instance.StartSpeedChange(giantSizeMultiplier, effectDuration);
                    break;
                case ItemType.Tiny:
                    // ũ�� �ſ� �۾��� (0.1��)
                    PlayerItemManager.instance.StartSizeChange(tinySizeMultiplier, effectDuration, true);
                    break;
            }

            // �������� ������ ������� �� �� ����
            Destroy(gameObject);
        }

    }

   
}
