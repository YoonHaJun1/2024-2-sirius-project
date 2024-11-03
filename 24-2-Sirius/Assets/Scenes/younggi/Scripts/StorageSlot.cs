using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageSlot
{
    [SerializeField]
    StorageItemData itemData;

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
}
