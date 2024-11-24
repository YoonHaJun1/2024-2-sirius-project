using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StorageSlot
{
    [SerializeField] private StorageItemData itemData;

    public StorageItemData ItemData => itemData;
    public StorageSlot()
    {
        ClearSlot();
    }
    public StorageSlot(StorageItemData source)
    {
        itemData = source;
    }

    public void ClearSlot()
    {
        itemData = null;
    }

    public void UpdateStorageSlot(StorageItemData data)
    {
        itemData = data;
    }
}
