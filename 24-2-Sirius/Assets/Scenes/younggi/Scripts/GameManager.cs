using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        buy, magic, sell, result
    }

    public State state;
    int sellerCount;
    int buyerCount;
    public int itemCount = 0;
    private int preItemCount = 0;

    private void Awake()
    {
        startBuyItem();
    }

    void startBuyItem()
    {
        state = State.buy;
        sellerCount = 5;

        Debug.Log("구매 단계");
    }

    void handleBuy()
    {
        sellerCount -= 1;


        // Debug.Log("buy Item");

        if (sellerCount > 0)
        {
            // Debug.Log("more customers");
        }
        else
        {
            // Debug.Log("No more customers");
            sellerCount = 0;
            startMagicItem();
        }
    }

    void startMagicItem()
    {
        state = State.magic;

        Debug.Log("해주 단계");

        preItemCount = itemCount;
    }

    void handleMagic()
    {
        if (preItemCount <= 0)
        {
            startSellItem();
        }

        preItemCount -= 1;
    }

    void startSellItem()
    {
        state = State.sell;

        Debug.Log("판매 단계");

        preItemCount = itemCount;
    }

    void handleSell()
    {
        if (preItemCount <= 0)
        {
            Debug.Log("다 팔았음");
            turnEnd();
        }

        preItemCount -= 1;
    }

    void turnEnd()
    {
        state = State.result;
    }

    public void handleAction()
    {
        if (state == State.buy)
        {
            Debug.Log("buy!");
            handleBuy();
        }
        else if (state == State.magic)
        {
            Debug.Log("magic!");
            handleMagic();
        }
        else if (state == State.sell)
        {
            Debug.Log("sell!!");
            handleSell();
        }
        else if (state == State.result)
        {
            Debug.Log("end!!!!!!");
        }
    }
}
