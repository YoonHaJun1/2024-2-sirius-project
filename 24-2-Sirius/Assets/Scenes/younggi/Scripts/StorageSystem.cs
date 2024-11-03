using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;


public class StorageSystem
{
    [SerializeField] List<StorageSlot> storageSlots;

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
}
