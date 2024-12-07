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

    public bool HasItem()
    {
        StorageSlot notRemainSlot = StorageSlots.FirstOrDefault(i => i.ItemData != null);
        return notRemainSlot != null;
    }

     public bool RemoveItem(StorageItemData itemToRemove)
    {
        StorageSlot slotToRemove = storageSlots.FirstOrDefault(i => i.ItemData == itemToRemove);
        if (slotToRemove != null)
        {
            slotToRemove.ClearSlot(); // Clear the slot
            RearrangeStorage();
            //Debug.Log("Item removed");
            return true;
        }
        //Debug.Log("Item to remove not found");
        return false; // Item not found in storage
    }

    private void RearrangeStorage()
    {
        int slotcount = 0;
        int lastIndex = 0;
        for (int i = 0; i < StorageSlots.Count; i++)
        {
            var check = StorageSlots[i];
            if (check.ItemData != null)
            {
                slotcount++;
                lastIndex = i;
            }
        }

        for (int i = 0; i < StorageSlots.Count; i++)
        {
            var check = StorageSlots[i];
            if (check.ItemData == null && i != lastIndex)
            {
                storageSlots[i].UpdateStorageSlot(storageSlots[lastIndex].ItemData);
                storageSlots[lastIndex].ClearSlot(); 
                break;
            }
        }

    //     for (int i = 0; i < )

    //     for (int i = 0; i < storageSlots.Count - 1; i++)
    //     {
    //         if (storageSlots[i].ItemData == null) // Find the first empty slot
    //         {
    //             int lastIndex = storageSlots.Count - 1;

    //             // Move the last item's data to the empty slot
    //             if (storageSlots[lastIndex].ItemData != null)
    //             {
    //                 storageSlots[i].UpdateStorageSlot(storageSlots[lastIndex].ItemData);
    //                 storageSlots[lastIndex].ClearSlot(); 
    //             }
    //         }
    //     }
    // }
    }
}
