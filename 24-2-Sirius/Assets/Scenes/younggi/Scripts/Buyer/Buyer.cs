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
    public double profitRatio;
    private StorageSystem storageSystem; // Reference to the StorageSystem
    private GameObject player;
    public StorageItemData selectedItem;
    
    private void Awake()
    {
        patienceLevel = Random.Range(2, 5);
        itemIndex = 0;
        priceLevel = Random.Range(0, 6);
        GoodorBadDecide();
        SetPlayer(GameObject.Find("Player"));
        buyerMoney =0; //Buyer 현재 돈
        DecideItem(); //Buyer 아이탬 구매 선택
        Debug.Log("isGoodBuyer: " +isGoodBuyer);
        Debug.Log("profitRatio: " + profitRatio);
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


        List<StorageItemData> affordableItems = null;
        
        if(storageSystem.HasItem()){
            foreach (var slot in storageSystem.StorageSlots)
            {
                if(slot.ItemData != null && slot.ItemData.value <= buyerMoney){
                    affordableItems.Add(slot.ItemData);
                }
            }
 
            // affordableItems = storageSystem.StorageSlots
            // .Where(slot => slot.ItemData != null && slot.ItemData.value <= buyerMoney)
            // .Select(slot => slot.ItemData)
            // .ToList();
        }

        // foreach (var item in affordableItems)
        // {
        //     Debug.Log($"Affordable Item Value: {item.value}");
        // }

        if(affordableItems == null){
            selectedItem = null;
        }else{
            Debug.Log($"{affordableItems.Count} items.");

            int randomIndex = Random.Range(0, affordableItems.Count);
            selectedItem = affordableItems[randomIndex];
            Debug.Log($"Selected Item Value: {selectedItem.value}");
            Debug.Log($"{(selectedItem.value*profitRatio)}원에 살게요");
        }

        
    }

    public void GoodorBadDecide()
    {
        int goodorBad = Random.Range(1, 100);

        Debug.Log("goodorBad amount: " + goodorBad);
        if (goodorBad < 90)
        {
            isGoodBuyer = true;
            profitRatio = Random.Range(0.8f, 1.2f);
        }
        else
        {
            isGoodBuyer = false;
            profitRatio = Random.Range(0.1f, 0.7f);
        }
    }
}

