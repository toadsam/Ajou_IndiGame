using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("UI Components")]
    public GameObject inventoryUI;       // 인벤토리 UI
    public Transform inventoryListParent; // 아이템 목록 부모 객체
    public GameObject inventorySlotPrefab; // 인벤토리 슬롯 프리팹

    public static bool isInventoryActive;


    [Header("Item Detail UI")]
    public GameObject itemDetailUI;          // 아이템 상세 정보 UI
    public TextMeshProUGUI detailItemName;   // 상세 정보: 아이템 이름
    public TextMeshProUGUI detailItemDescription; // 상세 정보: 아이템 설명
    public Image detailItemIcon;            // 상세 정보: 아이템 아이콘
    public Button equipButton;              // 장착 버튼
    public Button closeButton;

    private InventoryItem selectedItem;     // 현재 선택된 아이템

    [Header("Equipment Settings")]
    public GameObject equipmentParent; // 장착 아이템 부모 오브젝트

    [Header("Inventory Data")]
    public List<InventoryItem> inventoryItems = new List<InventoryItem>(); // 인벤토리 아이템 목록

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        if (itemDetailUI != null)
        {
            itemDetailUI.SetActive(false); // 상세 정보 UI 비활성화
        }

        InitializeEquipment(); // 장착 상태 초기화
    }



    private void Update()
    { 
        
            // I 키를 눌렀을 때 인벤토리 열기/닫기
        if (Input.GetKeyDown(KeyCode.I))
          {
                ToggleInventory();
          }

        // Store UI가 활성화된 동안 마우스 커서를 활성화
        if (inventoryUI != null && inventoryUI.activeSelf)
        {
            isInventoryActive = true;
        }
        else
        {
            isInventoryActive = false;
        }
    }

    public void AddItemToInventory(InventoryItem item)
    {
        inventoryItems.Add(item); // 데이터 추가
        UpdateInventoryUI(); // UI 갱신
    }

    public void ShowItemDetail(InventoryItem item)
    {
        selectedItem = item;

        // 상세 정보 UI 업데이트
        detailItemName.text = item.itemName;
        detailItemDescription.text = item.itemDescription;

        if (detailItemIcon != null)
        {
            detailItemIcon.sprite = item.itemIcon; // 아이템 아이콘 설정
            detailItemIcon.gameObject.SetActive(true); // 이미지 활성화
        }

        // 상세 정보 UI 표시
        itemDetailUI.SetActive(true);

        // 버튼 동작 설정
        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(() => EquipItem());

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => CloseItemDetail());
    }

    private void EquipItem()
    {
        if (equipmentParent == null)
        {
            Debug.LogError("Equipment Parent가 설정되지 않았습니다!");
            return;
        }

        // 모든 자식 비활성화
        foreach (Transform child in equipmentParent.transform)
        {
            child.gameObject.SetActive(false);
        }

        // 선택된 아이템 활성화
        Transform targetItem = equipmentParent.transform.Find(selectedItem.itemName);
        if (targetItem != null)
        {
            targetItem.gameObject.SetActive(true);
            Debug.Log($"{selectedItem.itemName} 장착되었습니다!");

            // PlayerController의 ChangeWeapon 호출
            PlayerController.instance.ChangeWeapon(selectedItem.itemName);
        }
        else
        {
            Debug.LogError($"'{selectedItem.itemName}' 아이템을 찾을 수 없습니다. 장착 실패.");
        }

        // 상세 정보 UI 닫기
        CloseItemDetail();
    }


    private void CloseItemDetail()
    {
        itemDetailUI.SetActive(false);
    }


    private void UpdateInventoryUI()
    {
        // 기존 아이템 슬롯 삭제
        foreach (Transform child in inventoryListParent)
        {
            Destroy(child.gameObject);
        }

        // 새로운 아이템 슬롯 생성
        foreach (var item in inventoryItems)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryListParent);

            // 아이템 이름 및 아이콘 설정
            var textComponent = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null) textComponent.text = item.itemName;

            var imageComponent = slot.GetComponent<Image>();
            if (imageComponent != null) imageComponent.sprite = item.itemIcon;

            // 클릭 이벤트 설정
            slot.GetComponent<Button>().onClick.AddListener(() => ShowItemDetail(item));
        }
    }
    private void InitializeEquipment()
    {
        if (equipmentParent != null)
        {
            foreach (Transform child in equipmentParent.transform)
            {
                child.gameObject.SetActive(false); // 모든 자식 비활성화
            }
        }
    }



    public void ToggleInventory()
    {
        bool isActive = inventoryUI.activeSelf;
        inventoryUI.SetActive(!isActive); // 인벤토리 UI 열기/닫기
    }
}
