using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("UI Components")]
    public GameObject inventoryUI;       // �κ��丮 UI
    public Transform inventoryListParent; // ������ ��� �θ� ��ü
    public GameObject inventorySlotPrefab; // �κ��丮 ���� ������

    public static bool isInventoryActive;


    [Header("Item Detail UI")]
    public GameObject itemDetailUI;          // ������ �� ���� UI
    public TextMeshProUGUI detailItemName;   // �� ����: ������ �̸�
    public TextMeshProUGUI detailItemDescription; // �� ����: ������ ����
    public Image detailItemIcon;            // �� ����: ������ ������
    public Button equipButton;              // ���� ��ư
    public Button closeButton;

    private InventoryItem selectedItem;     // ���� ���õ� ������

    [Header("Equipment Settings")]
    public GameObject equipmentParent; // ���� ������ �θ� ������Ʈ

    [Header("Inventory Data")]
    public List<InventoryItem> inventoryItems = new List<InventoryItem>(); // �κ��丮 ������ ���

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        if (itemDetailUI != null)
        {
            itemDetailUI.SetActive(false); // �� ���� UI ��Ȱ��ȭ
        }

        InitializeEquipment(); // ���� ���� �ʱ�ȭ
    }



    private void Update()
    { 
        
            // I Ű�� ������ �� �κ��丮 ����/�ݱ�
        if (Input.GetKeyDown(KeyCode.I))
          {
                ToggleInventory();
          }

        // Store UI�� Ȱ��ȭ�� ���� ���콺 Ŀ���� Ȱ��ȭ
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
        inventoryItems.Add(item); // ������ �߰�
        UpdateInventoryUI(); // UI ����
    }

    public void ShowItemDetail(InventoryItem item)
    {
        selectedItem = item;

        // �� ���� UI ������Ʈ
        detailItemName.text = item.itemName;
        detailItemDescription.text = item.itemDescription;

        if (detailItemIcon != null)
        {
            detailItemIcon.sprite = item.itemIcon; // ������ ������ ����
            detailItemIcon.gameObject.SetActive(true); // �̹��� Ȱ��ȭ
        }

        // �� ���� UI ǥ��
        itemDetailUI.SetActive(true);

        // ��ư ���� ����
        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(() => EquipItem());

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => CloseItemDetail());
    }

    private void EquipItem()
    {
        if (equipmentParent == null)
        {
            Debug.LogError("Equipment Parent�� �������� �ʾҽ��ϴ�!");
            return;
        }

        // ��� �ڽ� ��Ȱ��ȭ
        foreach (Transform child in equipmentParent.transform)
        {
            child.gameObject.SetActive(false);
        }

        // ���õ� ������ Ȱ��ȭ
        Transform targetItem = equipmentParent.transform.Find(selectedItem.itemName);
        if (targetItem != null)
        {
            targetItem.gameObject.SetActive(true);
            Debug.Log($"{selectedItem.itemName} �����Ǿ����ϴ�!");

            // PlayerController�� ChangeWeapon ȣ��
            PlayerController.instance.ChangeWeapon(selectedItem.itemName);
        }
        else
        {
            Debug.LogError($"'{selectedItem.itemName}' �������� ã�� �� �����ϴ�. ���� ����.");
        }

        // �� ���� UI �ݱ�
        CloseItemDetail();
    }


    private void CloseItemDetail()
    {
        itemDetailUI.SetActive(false);
    }


    private void UpdateInventoryUI()
    {
        // ���� ������ ���� ����
        foreach (Transform child in inventoryListParent)
        {
            Destroy(child.gameObject);
        }

        // ���ο� ������ ���� ����
        foreach (var item in inventoryItems)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryListParent);

            // ������ �̸� �� ������ ����
            var textComponent = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null) textComponent.text = item.itemName;

            var imageComponent = slot.GetComponent<Image>();
            if (imageComponent != null) imageComponent.sprite = item.itemIcon;

            // Ŭ�� �̺�Ʈ ����
            slot.GetComponent<Button>().onClick.AddListener(() => ShowItemDetail(item));
        }
    }
    private void InitializeEquipment()
    {
        if (equipmentParent != null)
        {
            foreach (Transform child in equipmentParent.transform)
            {
                child.gameObject.SetActive(false); // ��� �ڽ� ��Ȱ��ȭ
            }
        }
    }



    public void ToggleInventory()
    {
        bool isActive = inventoryUI.activeSelf;
        inventoryUI.SetActive(!isActive); // �κ��丮 UI ����/�ݱ�
    }
}
