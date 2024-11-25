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
    public TextMeshProUGUI selectedItemName;     // 선택된 아이템 이름
    public TextMeshProUGUI selectedItemDescription; // 선택된 아이템 설명
    public TextMeshProUGUI selectedItemPrice;    // 선택된 아이템 가격
    public Button buyButton;          // 구매 버튼

    public static bool isActicve; 

    [Header("Store Items")]
    public List<SchoolItem> storeItems = new List<SchoolItem>(); // 상점에서 판매할 아이템 목록

    private SchoolItem selectedItem; // 현재 선택된 아이템

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        isActicve = false;
    }

    private void Start()
    {
        InitializeStore();
        storeUI.SetActive(false); // 초기에는 상점 UI 비활성화
    }

    private void Update()
    {
        // Store UI가 활성화된 동안 마우스 커서를 활성화
        if (storeUI != null && storeUI.activeSelf)
        {
            isActicve = true;
        }
        else
        {
            isActicve=false;
        }
    }

    private void InitializeStore()
    {
        foreach (var item in storeItems)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemListParent);
            slot.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName; // 아이템 이름 표시
            slot.GetComponent<Button>().onClick.AddListener(() => SelectItem(item)); // 클릭 시 아이템 선택
        }
    }

    private void SelectItem(SchoolItem item)
    {
        selectedItem = item;
        selectedItemName.text = item.itemName;
        selectedItemDescription.text = item.itemDescription;
        selectedItemPrice.text = $"Price: {item.itemPrice}";

        buyButton.interactable = true; // 구매 버튼 활성화
    }

    public void BuySelectedItem()
    {
        if (selectedItem != null && PlayerStats.instance.SpendGold(selectedItem.itemPrice))
        {
            Debug.Log($"{selectedItem.itemName}을(를) 구매했습니다!");

            // 인벤토리에 아이템 추가
            InventoryManager.instance.AddItemToInventory(new InventoryItem
            {
                itemName = selectedItem.itemName,
                itemDescription = selectedItem.itemDescription,
                itemIcon = selectedItem.itemIcon,
                itemPrice = selectedItem.itemPrice
            });
        }
        else
        {
            Debug.Log("재화가 부족합니다.");
        }
    }

    public void ToggleStore()
    {
        bool isActive = storeUI.activeSelf;
        storeUI.SetActive(!isActive); // 상점 UI 활성화/비활성화
    }
}
