using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Buyer : MonoBehaviour
{
    public int priceLevel;
    public bool isGoodBuyer;
    //public int buyerMoney;
    public int patienceLevel; //흥정 받아들이는 횟수
    private int firstpatienceLevel;
    public double profitRatio;
    private StorageSystem storageSystem; // Reference to the StorageSystem
    private GameObject player;
    public StorageItemData selectedItem;
    private double biggestcall;  
    private void Awake()
    {
        //buyerMoney = 100; //Buyer 현재 돈
        patienceLevel = Random.Range(3, 8);
        Debug.Log("patiencelevel : " + patienceLevel);
        firstpatienceLevel = patienceLevel;
        priceLevel = Random.Range(0, 6);
        GoodorBadDecide();
        SetPlayer(GameObject.Find("Player"));
        DecideItem();
        Debug.Log("isGoodBuyer: " + isGoodBuyer);
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

        int randomcount = -1;

        for (int i = 0; i < storageSystem.StorageSlots.Count; i++)
        {
            var check = storageSystem.StorageSlots[i];
            if (check.ItemData != null)
            {
                randomcount++;
            }
        }

        int randomIndex = Random.Range(0, randomcount);
        var slot = storageSystem.StorageSlots[randomIndex];
        selectedItem = slot.ItemData;
        Debug.Log($"Selected Item Value: {selectedItem.value}");
        Debug.Log($"{(selectedItem.value*profitRatio)}원에 살게요");
    }

    public void GoodorBadDecide()
    {
        int goodorBad = Random.Range(1, 100);

        if (goodorBad < 80)
        {
            isGoodBuyer = true;
            profitRatio = Random.Range(0.8f, 1.3f);
        }
        else
        {
            isGoodBuyer = false;
            profitRatio = Random.Range(0.5f, 0.7f);
            
        }
        profitRatio = System.Math.Truncate(profitRatio*1000)/1000;
    }

    public void Bargain(int suggested)
    {
        double valuedif = (double)suggested/(double)selectedItem.value;
        Debug.Log("Valuedif: "+ valuedif);
        double add = 0.0f;
        if (isGoodBuyer == false)
        {
            BadBargain(suggested);
        }
        if (firstpatienceLevel <= 4 && isGoodBuyer == true)
        {
            if (profitRatio <= 1)
            {
                if (valuedif >= 0.2 && valuedif < 0.6)
                {
                    add = Random.Range(-0.1f, 0.1f);
                }
                if (valuedif < 0.7)
                {
                    add = Random.Range(-0.05f, 0.15f);
                }
                else if (0.7 <= valuedif && valuedif <= 1)
                {
                    profitRatio = Random.Range((float)profitRatio -0.05f, (float)valuedif);
                    profitRatio = System.Math.Truncate(profitRatio*1000)/1000;
                }
                else if(1 < valuedif && valuedif <= 1.7)
                {
                    profitRatio = Random.Range((float)profitRatio, (float)valuedif);
                    profitRatio = System.Math.Truncate(profitRatio*1000)/1000;
                }
                else
                {
                    add = Random.Range(-0.1f, -0.15f);
                }
            }
            else if (profitRatio > 1)
            {
                if (valuedif >= 0.2 && valuedif < 0.6)
                {
                    add = Random.Range(-0.2f, 0.1f);
                }
                if (valuedif < 0.7)
                {
                    add = Random.Range(-0.1f, 0.1f);
                }
                else if (0.7 <= valuedif && valuedif <= 1)
                {
                    profitRatio = Random.Range((float)profitRatio -0.05f, (float)valuedif);
                    profitRatio = System.Math.Truncate(profitRatio*1000)/1000;
                }
                else if(1 < valuedif && valuedif <= 1.7)
                {
                    profitRatio = Random.Range((float)profitRatio - 0.1f, (float)valuedif);
                    profitRatio = System.Math.Truncate(profitRatio*1000)/1000;
                }
                else
                {
                    add = Random.Range(-0.1f, -0.2f);
                }
            }
        }
        else if (firstpatienceLevel > 4 && isGoodBuyer == true)
        {
            if (profitRatio < 1.1)
            {
                if (valuedif >= 0.2 && valuedif < 0.6)
                {
                    add = Random.Range(-0.15f, 0f);
                }
                if (valuedif < 0.7)
                {
                    add = Random.Range(-0.1f, 0.05f);
                }
                else if (0.7 <= valuedif && valuedif <= 1)
                {
                    profitRatio = Random.Range((float)profitRatio -0.05f, (float)valuedif);
                    profitRatio = System.Math.Truncate(profitRatio*1000)/1000;
                }
                else if(1 < valuedif && valuedif <= 1.4)
                {
                    profitRatio = Random.Range((float)profitRatio, (float)valuedif);
                    profitRatio = System.Math.Truncate(profitRatio*1000)/1000;
                }
                else
                {
                    add = Random.Range(-0.05f, -0.2f);
                }
            }
            else if (profitRatio >= 1.1)
            {
                if (valuedif >= 0.2 && valuedif < 0.6)
                {
                    add = Random.Range(-0.2f, 0f);
                }
                if (valuedif < 0.7)
                {
                    add = Random.Range(-0.1f, 0.05f);
                }
                else if (0.7 <= valuedif && valuedif <= 1)
                {
                    profitRatio = Random.Range((float)profitRatio -0.2f, (float)valuedif);
                    profitRatio = System.Math.Truncate(profitRatio*1000)/1000;
                }
                else if(1 < valuedif && valuedif <= 1.4)
                {
                    profitRatio = Random.Range((float)profitRatio - 0.1f, (float)valuedif);
                    profitRatio = System.Math.Truncate(profitRatio*1000)/1000;
                }
                else
                {
                    add = Random.Range(-0.2f,-0.15f);
                }
            }
        }
        
        add = System.Math.Truncate(add*1000)/1000;
        Debug.Log("add amount: " + add);
        profitRatio += add;
        Debug.Log($"{(selectedItem.value*profitRatio)}원에 살게요");
    }

    public void BadBargain(int suggest)
    {
        double valuedif = suggest/selectedItem.value;
        double add = 0;
        if (valuedif < 1)
        {
            profitRatio = Random.Range((float)profitRatio - 0.1f, (float)valuedif);
            profitRatio = System.Math.Truncate(profitRatio*1000)/1000;
        }
        else if (valuedif >= 1)
        {
            profitRatio = Random.Range((float)profitRatio, 0.9f);
        }
    }
}

