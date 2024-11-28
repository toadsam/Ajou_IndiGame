using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance;

    [Header("UI Components")]
    public GameObject storeUI;         // 상점 UI
    public Transform itemListParent;  // 아이템 목록 부모 객체
    public GameObject itemSlotPrefab; // 아이템 슬롯 프리팹

    [Header("Store Items")]
    public List<SchoolItem> storeItems = new List<SchoolItem>(); // 상점에서 판매할 아이템 목록

    public static bool isActive = false; // 상점 활성화 상태 (static)

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitializeStore();
        storeUI.SetActive(false); // 초기에는 상점 UI 비활성화
        isActive = false; // 초기 상태
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player와 충돌하면 상점 UI 활성화
        if (other.CompareTag("Player"))
        {
            ToggleStore();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Player가 상점 범위를 벗어나면 상점 UI 비활성화
        if (other.CompareTag("Player"))
        {
            ToggleStore();
        }
    }

    private void InitializeStore()
    {
        foreach (var item in storeItems)
        {
            // 슬롯 생성
            GameObject slot = Instantiate(itemSlotPrefab, itemListParent);

            // 슬롯 내용 설정
            var itemContainer = slot.transform.Find("Item"); // "Item" 자식 오브젝트 찾기
            var BuyButton = slot.transform.Find("BuyButton"); // "Item" 자식 오브젝트 찾기
            var price = BuyButton.Find("Price").GetComponent<TextMeshProUGUI>(); // "ItemImage" 컴포넌트
            var slotImage = itemContainer.Find("ItemImage").GetComponent<Image>(); // "ItemImage" 컴포넌트
            var slotText = slot.transform.Find("ItemName").GetComponent<TextMeshProUGUI>(); // "ItemName" 컴포넌트
            var slotDescription = slot.transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>(); // "ItemDescription"
            var buyButton = slot.transform.Find("BuyButton").GetComponent<Button>(); // "BuyButton"

            slotImage.sprite = item.itemIcon; // 아이템 이미지 설정
            slotText.text = item.itemName; // 아이템 이름 설정
            slotDescription.text = item.itemDescription; // 아이템 설명 설정
            price.text = item.itemPrice.ToString();

            // 구매 버튼 동작 설정
            buyButton.onClick.AddListener(() => BuyItem(item));
        }
    }

    private void BuyItem(SchoolItem item)
    {
        if (PlayerStats.instance.SpendGold(item.itemPrice))
        {
            Debug.Log($"{item.itemName}을(를) 구매했습니다!");

            // 인벤토리에 아이템 추가
            InventoryManager.instance.AddItemToInventory(new InventoryItem
            {
                itemName = item.itemName,
                itemDescription = item.itemDescription,
                itemIcon = item.itemIcon,
                itemPrice = item.itemPrice
            });
        }
        else
        {
            Debug.Log("재화가 부족합니다.");
        }
    }

    public void ToggleStore()
    {
        isActive = !isActive; // 상점 활성화 상태 토글
        storeUI.SetActive(isActive); // 상점 UI 활성화/비활성화
    }
}
