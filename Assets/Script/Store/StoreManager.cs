using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance;

    [Header("UI Components")]
    public GameObject storeUI;         // ���� UI
    public Transform itemListParent;  // ������ ��� �θ� ��ü
    public GameObject itemSlotPrefab; // ������ ���� ������
    public Text selectedItemName;     // ���õ� ������ �̸�
    public Text selectedItemDescription; // ���õ� ������ ����
    public Text selectedItemPrice;    // ���õ� ������ ����
    public Button buyButton;          // ���� ��ư

    [Header("Store Items")]
    public List<Item> storeItems = new List<Item>(); // �������� �Ǹ��� ������ ���

    private Item selectedItem; // ���� ���õ� ������

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitializeStore();
        storeUI.SetActive(false); // �ʱ⿡�� ���� UI ��Ȱ��ȭ
    }

    private void InitializeStore()
    {
        foreach (var item in storeItems)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemListParent);
            slot.GetComponentInChildren<Text>().text = item.itemName; // ������ �̸� ǥ��
            slot.GetComponent<Button>().onClick.AddListener(() => SelectItem(item)); // Ŭ�� �� ������ ����
        }
    }

    private void SelectItem(Item item)
    {
        selectedItem = item;
        selectedItemName.text = item.itemName;
        selectedItemDescription.text = item.itemDescription;
        selectedItemPrice.text = $"Price: {item.itemPrice}";

        buyButton.interactable = true; // ���� ��ư Ȱ��ȭ
    }

   /* public void BuySelectedItem()
    {
        if (selectedItem != null && PlayerData.instance.SpendGold(selectedItem.itemPrice))
        {
            Debug.Log($"{selectedItem.itemName}��(��) �����߽��ϴ�!");
        }
        else
        {
            Debug.Log("��ȭ�� �����մϴ�.");
        }
    }*/

    public void ToggleStore()
    {
        bool isActive = storeUI.activeSelf;
        storeUI.SetActive(!isActive); // ���� UI Ȱ��ȭ/��Ȱ��ȭ
    }
}
