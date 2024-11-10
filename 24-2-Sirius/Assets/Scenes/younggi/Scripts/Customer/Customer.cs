using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public int difficultyLevel;
    public int patienceLevel;
    public StorageItemData itemData;
    public Sprite charaImage;

    private int itemId;
    private StorageItemData.CurseType curseType;
    private StorageItemData.BlessType blessType;
    public int value;
    private int blessMaxLimit;

    private void Awake()
    {
        difficultyLevel = Random.Range(0, 6);
        patienceLevel = Random.Range(0, 6);
        // itemData = new StorageItemData(itemId, curseType, blessType, value, blessMaxLimit);
        itemData = ScriptableObject.CreateInstance<StorageItemData>();
        itemData.init(itemId, curseType, blessType, value, blessMaxLimit);

        itemId = Random.Range(0, 6);
        curseType = (StorageItemData.CurseType)Random.Range(0, 6);
        blessType = (StorageItemData.BlessType)Random.Range(0, 6);
        value = Random.Range(0, 100);
        blessMaxLimit = Random.Range(0, 6);
    }
}
