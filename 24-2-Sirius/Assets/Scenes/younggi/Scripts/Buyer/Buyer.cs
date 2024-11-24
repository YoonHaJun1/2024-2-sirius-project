using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buyer : MonoBehaviour
{
    public int itemIndex;
    public int priceLevel;
    public bool isGoodBuyer;

    private void Awake()
    {
        itemIndex = 0;
        priceLevel = Random.Range(0, 6);
        isGoodBuyer = Random.Range(0, 2) == 1;
    }
}
