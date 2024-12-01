using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;


[System.Serializable]
public class StorageSystem
{
    [SerializeField] private List<StorageSlot> storageSlots;

    public List<StorageSlot> StorageSlots => storageSlots;
    public int StorageSize => StorageSlots.Count;

    public UnityAction<StorageSlot> OnStorageSlotChanged;

    public StorageSystem(int size)
    {
        storageSlots = new List<StorageSlot>(size);

        for (int i = 0; i < size; i++)
        {
            storageSlots.Add(new StorageSlot());
        }
    }

    public bool AddItem(StorageItemData addedItem)
    {
        if (HasFreeSlot(out StorageSlot freeSlot))
        {
            freeSlot.UpdateStorageSlot(addedItem);
            OnStorageSlotChanged?.Invoke(freeSlot);
            return true;
        }

        return false;
    }

    private bool HasFreeSlot(out StorageSlot freeSlot)
    {
        freeSlot = StorageSlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot != null;
    }

    public bool HasSlot()
    {
        StorageSlot remainSlot = StorageSlots.FirstOrDefault(i => i.ItemData == null);
        return remainSlot != null;
    }

     public bool RemoveItem(StorageItemData itemToRemove)
    {
        StorageSlot slotToRemove = storageSlots.FirstOrDefault(i => i.ItemData == itemToRemove);
        if (slotToRemove != null)
        {
            slotToRemove.ClearSlot(); // Clear the slot
            //Debug.Log("Item removed");
            return true;
        }
        //Debug.Log("Item to remove not found");
        return false; // Item not found in storage
    }
}
