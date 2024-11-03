using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StorageItemData", menuName = "Storage System/Storage Item", order = 0)]
public class StorageItemData : ScriptableObject
{
    public int id;
    public string itemName;
    [TextArea(4, 4)]
    public string description;
    public Sprite Icon;
}

