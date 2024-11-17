using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    enum State
    {
        buy, magic, sell, result
    }

    [SerializeField] State state;

    [SerializeField] TextMeshProUGUI stateTextObject;
    [SerializeField] TextMeshProUGUI wallet;

    [SerializeField] InputField inputField;


    private int round = 0;
    private int money = 10000;
    public int moneytaken = 0;
    private int sellerCount = 0;
    private int buyerCount = 0;
    private int itemCount = 0;
    private int preItemCount = 0;


    private void Awake()
    {
        startBuyItem();
    }

    private void Update()
    {
        wallet.text = money.ToString();
    }

    private void startBuyItem()
    {
        state = State.buy;

        round += 1;
        sellerCount = 5;

        stateTextObject.text = "구매단계";
    }


    public void ValidateInput()
    {
        string input = inputField.text;
        int.TryParse(input, out moneytaken);
        Debug.Log(input);
        Debug.Log(moneytaken);
        money -= moneytaken;
    }


    private void handleBuy()
    {
        sellerCount -= 1;

        money -= 100;

        if (sellerCount > 0)
        {
        }
        else
        {

            sellerCount = 0;
            startMagicItem();
        }
    }

    private void startMagicItem()
    {
        state = State.magic;

        stateTextObject.text = "해주단계";

        preItemCount = itemCount;
    }

    private void handleMagic()
    {
        if (preItemCount <= 0)
        {
            startSellItem();
        }

        preItemCount -= 1;
    }

    private void startSellItem()
    {
        state = State.sell;

        stateTextObject.text = "판매단계";

        buyerCount = 5;
    }

    private void handleSell()
    {
        money += 200;

        if (buyerCount <= 0)
        {
            turnEnd();
        }

        buyerCount -= 1;
    }

    private void turnEnd()
    {
        state = State.result;

        stateTextObject.text = "정비";
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
            startBuyItem();
        }
    }
}
