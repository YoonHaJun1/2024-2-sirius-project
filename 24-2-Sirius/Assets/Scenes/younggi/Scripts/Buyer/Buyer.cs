using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Buyer : MonoBehaviour
{
    
    public int itemIndex;
    public int priceLevel;
    public bool isGoodBuyer;
    public int buyerMoney;
    public int patienceLevel; //흥정 받아들이는 횟수
    public int requestmoney;
    private StorageSystem storageSystem; // Reference to the StorageSystem
    private GameObject player;
    public StorageItemData selectedItem;
    
    private void Awake()
    {
        patienceLevel = Random.Range(2, 3);
        itemIndex = 0;
        priceLevel = Random.Range(0, 6);
        GoodorBadDecide();
        SetPlayer(GameObject.Find("Player"));
        buyerMoney = Random.Range(10, 100); //Buyer 현재 돈
        DecideItem(); //Buyer 아이탬 구매 선택
        DecidePrice(); //Buyer 가 제시할 금액
        Debug.Log(isGoodBuyer);
        Debug.Log(requestmoney);
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
        Debug.Log($"{affordableItems.Count} items.");

        foreach (var item in affordableItems)
        {
            Debug.Log($"Affordable Item Value: {item.value}");
        }

        int randomIndex = Random.Range(0, affordableItems.Count);
        selectedItem = affordableItems[randomIndex];
        Debug.Log($"Selected Item Value: {selectedItem.value}");
    }

    public void DecidePrice()
    {
        if (!isGoodBuyer) //사기꾼
        {
            requestmoney = Random.Range(-50,-1);
        }
        else //구매자
        {
            requestmoney = Random.Range(15, 50);
        }
    }

    public void GoodorBadDecide()
    {
        int goodorBad = 0;
        goodorBad = Random.Range(1, 100);
        if (goodorBad > 85)
        {
            isGoodBuyer = true;
        }
        else
        {
            isGoodBuyer = false;
        }
    }
}

