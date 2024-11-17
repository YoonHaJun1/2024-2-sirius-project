using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StorageItemData", menuName = "Storage System/Storage Item", order = 0)]
public class StorageItemData : ScriptableObject
{
    public enum CurseType
    {
        level1, level2, level3, level4
    }

    public enum BlessType
    {
        level1, level2, level3, level4
    }

    public int type;
    public string itemName;
    [TextArea(4, 4)]
    public string description;
    public Sprite itemImage;
    public CurseType curseType;
    public BlessType blessType;
    public int value;
    public int blessMaxLimit;

    public void init(int _type, CurseType _curseType, BlessType _blessType, int _value, int _blessMaxLimit)
    {
        type = _type;
        curseType = _curseType;
        blessType = _blessType;
        value = _value;
        blessMaxLimit = _blessMaxLimit;
    }
}

