using UnityEngine;

public class Item : MonoBehaviour

{
    // 아이템의 종류를 정의하는 enum
    public enum ItemType
    {
        Giant,  // 자이언트 아이템 (50배 커짐)
        Speed,   // 스피드 아이템 (10배 빨라짐)
        Tiny

    }

    public ItemType itemType; // 아이템 타입 설정

    public float giantSizeMultiplier = 30f;  // 자이언트 아이템 크기 배율
    public float tinySizeMultiplier = 0.1f;  // 작아지는 아이템 크기 배율
    public float speedMultiplier = 10f;      // 스피드 아이템 속도 배율
    public float effectDuration = 10f;       // 효과 지속 시간

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // 플레이어와 충돌하면
        {
            switch (itemType)
            {
                case ItemType.Giant:
                    // 크기 50배로 증가 + 힘 10배로 증가
                    StartCoroutine(PlayerItemManager.instance.ChangeSize(giantSizeMultiplier, effectDuration, true));
                    break;
                case ItemType.Speed:
                    // 속도 10배로 증가
                    StartCoroutine(PlayerItemManager.instance.ChangeSpeed(speedMultiplier, effectDuration));
                    break;
                case ItemType.Tiny:
                    // 크기 매우 작아짐 (0.1배)
                    StartCoroutine(PlayerItemManager.instance.ChangeSize(tinySizeMultiplier, effectDuration));
                    break;
            }

            // 아이템을 먹으면 사라지게 할 수 있음
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // 플레이어와 충돌하면
        {
            switch (itemType)
            {
                case ItemType.Giant:
                    // 크기 50배로 증가 + 힘 10배로 증가
                    StartCoroutine(PlayerItemManager.instance.ChangeSize(giantSizeMultiplier, effectDuration, true));
                    break;
                case ItemType.Speed:
                    // 속도 10배로 증가
                    StartCoroutine(PlayerItemManager.instance.ChangeSpeed(speedMultiplier, effectDuration));
                    break;
                case ItemType.Tiny:
                    // 크기 매우 작아짐 (0.1배)
                    StartCoroutine(PlayerItemManager.instance.ChangeSize(tinySizeMultiplier, effectDuration));
                    break;
            }

            // 아이템을 먹으면 사라지게 할 수 있음
            Destroy(gameObject);
        }
    }
}
