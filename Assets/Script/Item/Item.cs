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

    public float giantSizeMultiplier = 30f;  // ���̾�Ʈ ������ ũ�� ����
    public float tinySizeMultiplier = 0.1f;  // �۾����� ������ ũ�� ����
    public float speedMultiplier = 10f;      // ���ǵ� ������ �ӵ� ����
    public float effectDuration = 10f;       // ȿ�� ���� �ð�

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // �÷��̾�� �浹�ϸ�
        {
            switch (itemType)
            {
                case ItemType.Giant:
                    // ũ�� 50��� ���� + �� 10��� ����
                    StartCoroutine(PlayerItemManager.instance.ChangeSize(giantSizeMultiplier, effectDuration, true));
                    break;
                case ItemType.Speed:
                    // �ӵ� 10��� ����
                    StartCoroutine(PlayerItemManager.instance.ChangeSpeed(speedMultiplier, effectDuration));
                    break;
                case ItemType.Tiny:
                    // ũ�� �ſ� �۾��� (0.1��)
                    StartCoroutine(PlayerItemManager.instance.ChangeSize(tinySizeMultiplier, effectDuration));
                    break;
            }

            // �������� ������ ������� �� �� ����
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // �÷��̾�� �浹�ϸ�
        {
            switch (itemType)
            {
                case ItemType.Giant:
                    // ũ�� 50��� ���� + �� 10��� ����
                    StartCoroutine(PlayerItemManager.instance.ChangeSize(giantSizeMultiplier, effectDuration, true));
                    break;
                case ItemType.Speed:
                    // �ӵ� 10��� ����
                    StartCoroutine(PlayerItemManager.instance.ChangeSpeed(speedMultiplier, effectDuration));
                    break;
                case ItemType.Tiny:
                    // ũ�� �ſ� �۾��� (0.1��)
                    StartCoroutine(PlayerItemManager.instance.ChangeSize(tinySizeMultiplier, effectDuration));
                    break;
            }

            // �������� ������ ������� �� �� ����
            Destroy(gameObject);
        }
    }
}
