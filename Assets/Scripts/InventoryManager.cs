using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Data")]
    public List<Item> Items = new List<Item>();

    [Header("UI")]
    public GameObject inventoryPanel;
    public List<Image> slotImages = new List<Image>(); // slot icon image'larý (SIRALI)

    // Opsiyonel: equip gösterimi ayrý bir slot ise
    public Item equippedItem;
    public Image equippedItemSlot;

    private bool isInventoryOpen = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        // oyun baþýnda slotlarý temizle
        RefreshSlots();
        UpdateEquippedItemDisplay();
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.tabKey.wasPressedThisFrame)
            ToggleInventory();
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if (inventoryPanel != null)
            inventoryPanel.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            RefreshSlots();
            UpdateEquippedItemDisplay();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public bool Add(Item item)
    {
        if (item == null) return false;

        // envanter dolu mu kontrol
        if (Items.Count >= slotImages.Count)
        {
            Debug.LogWarning("Inventory full! Slot sayýsý yetmiyor.");
            return false;
        }

        Items.Add(item);
        RefreshSlots(); // item eklenince slotlarý güncelle
        return true;
    }

    public void Remove(Item item)
    {
        Items.Remove(item);

        if (equippedItem == item)
        {
            equippedItem = null;
            UpdateEquippedItemDisplay();
        }

        RefreshSlots();
    }

    public void EquipItem(Item item)
    {
        equippedItem = item;
        UpdateEquippedItemDisplay();
    }

    private void RefreshSlots()
    {
        // 1) tüm slot ikonlarýný kapat
        for (int i = 0; i < slotImages.Count; i++)
        {
            if (slotImages[i] == null) continue;
            slotImages[i].sprite = null;
            slotImages[i].enabled = false;
            var c = slotImages[i].color; c.a = 0f; slotImages[i].color = c;
        }

        // 2) item sayýsý kadarýný doldur
        for (int i = 0; i < Items.Count && i < slotImages.Count; i++)
        {
            var item = Items[i];
            var img = slotImages[i];
            if (img == null) continue;

            if (item != null && item.icon != null)
            {
                img.sprite = item.icon;
                img.enabled = true;
                var c = img.color; c.a = 1f; img.color = c;
            }
        }
    }

    private void UpdateEquippedItemDisplay()
    {
        if (equippedItemSlot == null) return;

        if (equippedItem != null && equippedItem.icon != null)
        {
            equippedItemSlot.sprite = equippedItem.icon;
            equippedItemSlot.enabled = true;
            var c = equippedItemSlot.color; c.a = 1f; equippedItemSlot.color = c;
        }
        else
        {
            equippedItemSlot.sprite = null;
            equippedItemSlot.enabled = false;
            var c = equippedItemSlot.color; c.a = 0f; equippedItemSlot.color = c;
        }
    }
}
