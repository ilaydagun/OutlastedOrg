using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item Item;
    public bool equipOnPickup = false;

    void Pickup()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager.Instance yok! Sahneye InventoryManager ekli mi?");
            return;
        }

        bool added = InventoryManager.Instance.Add(Item);
        if (!added) return; // eklenemediyse item yok olmas�n

        if (equipOnPickup)
            InventoryManager.Instance.EquipItem(Item);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Pickup();
    }
}
