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

    [Header("Store Items")]
    public List<SchoolItem> storeItems = new List<SchoolItem>(); // �������� �Ǹ��� ������ ���

    public static bool isActive = false; // ���� Ȱ��ȭ ���� (static)

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitializeStore();
        storeUI.SetActive(false); // �ʱ⿡�� ���� UI ��Ȱ��ȭ
        isActive = false; // �ʱ� ����
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player�� �浹�ϸ� ���� UI Ȱ��ȭ
        if (other.CompareTag("Player"))
        {
            ToggleStore();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Player�� ���� ������ ����� ���� UI ��Ȱ��ȭ
        if (other.CompareTag("Player"))
        {
            ToggleStore();
        }
    }

    private void InitializeStore()
    {
        foreach (var item in storeItems)
        {
            // ���� ����
            GameObject slot = Instantiate(itemSlotPrefab, itemListParent);

            // ���� ���� ����
            var itemContainer = slot.transform.Find("Item"); // "Item" �ڽ� ������Ʈ ã��
            var BuyButton = slot.transform.Find("BuyButton"); // "Item" �ڽ� ������Ʈ ã��
            var price = BuyButton.Find("Price").GetComponent<TextMeshProUGUI>(); // "ItemImage" ������Ʈ
            var slotImage = itemContainer.Find("ItemImage").GetComponent<Image>(); // "ItemImage" ������Ʈ
            var slotText = slot.transform.Find("ItemName").GetComponent<TextMeshProUGUI>(); // "ItemName" ������Ʈ
            var slotDescription = slot.transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>(); // "ItemDescription"
            var buyButton = slot.transform.Find("BuyButton").GetComponent<Button>(); // "BuyButton"

            slotImage.sprite = item.itemIcon; // ������ �̹��� ����
            slotText.text = item.itemName; // ������ �̸� ����
            slotDescription.text = item.itemDescription; // ������ ���� ����
            price.text = item.itemPrice.ToString();

            // ���� ��ư ���� ����
            buyButton.onClick.AddListener(() => BuyItem(item));
        }
    }

    private void BuyItem(SchoolItem item)
    {
        if (PlayerStats.instance.SpendGold(item.itemPrice))
        {
            Debug.Log($"{item.itemName}��(��) �����߽��ϴ�!");

            // �κ��丮�� ������ �߰�
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
            Debug.Log("��ȭ�� �����մϴ�.");
        }
    }

    public void ToggleStore()
    {
        isActive = !isActive; // ���� Ȱ��ȭ ���� ���
        storeUI.SetActive(isActive); // ���� UI Ȱ��ȭ/��Ȱ��ȭ
    }
}
