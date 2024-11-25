using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buyer : MonoBehaviour
{
    public int itemIndex;
    public int priceLevel;
    public bool isGoodBuyer;
    public int BuyerMoney;
    private int GoodorBad = 0;

    private void Awake()
    {
        itemIndex = 0;
        priceLevel = Random.Range(0, 6);
        GoodorBadDecide();
        BuyerMoney = Random.Range(1, 500);
    }

    public void GoodorBadDecide()
    {
        GoodorBad = Random.Range(1, 100);
        if (GoodorBad > 75)
        {
            isGoodBuyer = true;
        }
        else 
        {
            isGoodBuyer = false;
        }
    }

}
