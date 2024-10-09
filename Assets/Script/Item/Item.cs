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

    public float giantSizeMultiplier;  // 자이언트 아이템 크기 배율
    public float tinySizeMultiplier;  // 작아지는 아이템 크기 배율
    public float speedMultiplier;      // 스피드 아이템 속도 배율
    public float effectDuration;       // 효과 지속 시간

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // 플레이어와 충돌하면
        {
            switch (itemType)
            {
                case ItemType.Giant:
                    Debug.Log("1");
                    // 크기 50배로 증가 + 힘 10배로 증가
                    PlayerItemManager.instance.StartSizeChange(giantSizeMultiplier,effectDuration,true);
                    break;
                case ItemType.Speed:
                    // 속도 10배로 증가
                    PlayerItemManager.instance.StartSpeedChange(giantSizeMultiplier, effectDuration);
                    break;
                case ItemType.Tiny:
                    // 크기 매우 작아짐 (0.1배)
                    PlayerItemManager.instance.StartSizeChange(tinySizeMultiplier, effectDuration, true);
                    break;
            }

            // 아이템을 먹으면 사라지게 할 수 있음
            Destroy(gameObject);
        }

    }

   
}
