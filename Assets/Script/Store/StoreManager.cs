using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance;

    [Header("UI Components")]
    public GameObject storeUI;         // ���� UI
    public Transform itemListParent;  // ������ ��� �θ� ��ü
    public GameObject itemSlotPrefab; // ������ ���� ������
    public TextMeshProUGUI selectedItemName;     // ���õ� ������ �̸�
    public TextMeshProUGUI selectedItemDescription; // ���õ� ������ ����
    public TextMeshProUGUI selectedItemPrice;    // ���õ� ������ ����
    public Button buyButton;          // ���� ��ư

    public static bool isActicve; 

    [Header("Store Items")]
    public List<SchoolItem> storeItems = new List<SchoolItem>(); // �������� �Ǹ��� ������ ���

    private SchoolItem selectedItem; // ���� ���õ� ������

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        isActicve = false;
    }

    private void Start()
    {
        InitializeStore();
        storeUI.SetActive(false); // �ʱ⿡�� ���� UI ��Ȱ��ȭ
    }

    private void Update()
    {
        // Store UI�� Ȱ��ȭ�� ���� ���콺 Ŀ���� Ȱ��ȭ
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
            slot.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName; // ������ �̸� ǥ��
            slot.GetComponent<Button>().onClick.AddListener(() => SelectItem(item)); // Ŭ�� �� ������ ����
        }
    }

    private void SelectItem(SchoolItem item)
    {
        selectedItem = item;
        selectedItemName.text = item.itemName;
        selectedItemDescription.text = item.itemDescription;
        selectedItemPrice.text = $"Price: {item.itemPrice}";

        buyButton.interactable = true; // ���� ��ư Ȱ��ȭ
    }

    public void BuySelectedItem()
    {
        if (selectedItem != null && PlayerStats.instance.SpendGold(selectedItem.itemPrice))
        {
            Debug.Log($"{selectedItem.itemName}��(��) �����߽��ϴ�!");

            // �κ��丮�� ������ �߰�
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
            Debug.Log("��ȭ�� �����մϴ�.");
        }
    }

    public void ToggleStore()
    {
        bool isActive = storeUI.activeSelf;
        storeUI.SetActive(!isActive); // ���� UI Ȱ��ȭ/��Ȱ��ȭ
    }
}
