using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Buyer : MonoBehaviour
{
    
    public int itemIndex;
    public int priceLevel;
    public bool isGoodBuyer;
    public int buyerMoney;
    private int goodorBad = 0;
    private StorageSystem storageSystem; // Reference to the StorageSystem
    private GameObject player;

    private void Awake()
    {
        itemIndex = 0;
        priceLevel = Random.Range(0, 6);
        GoodorBadDecide();
        SetPlayer(GameObject.Find("Player"));
        buyerMoney = Random.Range(80, 100); // Buyer's available money
        DecideItem(); // Buyer selects an item to purchase
    }


    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject;
    }

    public void DecideItem()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is not set!");
            return;
        }

        // Logic to access StorageSystem from the player
        StorageHolder storageHolder = player.GetComponent<StorageHolder>();
        if (storageHolder == null) return;

        StorageSystem storageSystem = storageHolder.getStorageSystem();
        if (storageSystem == null) return;

        List<StorageItemData> affordableItems = storageSystem.StorageSlots
            .Where(slot => slot.ItemData != null && slot.ItemData.value <= buyerMoney)
            .Select(slot => slot.ItemData)
            .ToList();
        Debug.Log($"Buyer can afford {affordableItems.Count} items.");
    }

    public void GoodorBadDecide()
    {
        goodorBad = Random.Range(1, 100);
        isGoodBuyer = goodorBad > 75;
    }
}

