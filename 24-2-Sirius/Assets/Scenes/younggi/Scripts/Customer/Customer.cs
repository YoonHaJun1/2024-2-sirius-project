using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public int difficultyLevel;
    public int patienceLevel;
    public StorageItemData itemData;
    public Sprite charaImage;

    private int itemType;
    private StorageItemData.CurseType curseType;
    private StorageItemData.BlessType blessType;
    public int itemValue;
    private int blessMaxLimit;

    private void Awake()
    {
        difficultyLevel = Random.Range(0, 6);
        patienceLevel = Random.Range(0, 6);
        // itemData = new StorageItemData(itemType, curseType, blessType, itemValue, blessMaxLimit);
        itemData = ScriptableObject.CreateInstance<StorageItemData>();

        itemType = Random.Range(0, 6);
        curseType = (StorageItemData.CurseType)Random.Range(0, 5);
        blessType = (StorageItemData.BlessType)Random.Range(0, 5);

        itemValue = Random.Range(1, 100);
        Debug.Log(itemValue);
        blessMaxLimit = Random.Range(0, 6);

        itemData.init(itemType, curseType, blessType, itemValue, blessMaxLimit);
    }
}
