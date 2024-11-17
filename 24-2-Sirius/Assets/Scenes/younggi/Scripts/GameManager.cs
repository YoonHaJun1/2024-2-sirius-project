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
    
    [SerializeField] GameObject _customerPrefab;
    [SerializeField] GameObject cutomerSpwaner;
    [SerializeField] GameObject player;


    private int round = 0;
    private int money = 10000;
    public int moneytaken = 0; //흥정 입력값 (int)
    private int sellerCount = 0;
    private int buyerCount = 0;
    private int itemCount = 0;
    private int preItemCount = 0;

    private GameObject instantiatedCustomerObject;

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

        instantiatedCustomerObject = Instantiate(_customerPrefab, cutomerSpwaner.transform.position, Quaternion.identity);

        stateTextObject.text = "구매단계";
    }


    public void ValidateInput() //흥정 입력 값 저장 및 지갑에서 돈 빼기
    {
        string input = inputField.text; //흥정 요구 값 저장
        int.TryParse(input, out moneytaken); //string -> moneytaken int 값으로 변환
        Debug.Log(input);
        Debug.Log(moneytaken);
        money -= moneytaken; 
    }


    private void handleBuy()
    {
        sellerCount -= 1;

        Customer customer = instantiatedCustomerObject.GetComponent<Customer>();


        if (player.GetComponent<StorageHolder>().getStorageSystem().AddItem(customer.itemData) == true)
        {
            money -= customer.itemData.value;
        }


        Destroy(instantiatedCustomerObject);

        if (sellerCount > 0)
        {

            instantiatedCustomerObject = Instantiate(_customerPrefab, cutomerSpwaner.transform.position, Quaternion.identity);
        }
        else
        {
            instantiatedCustomerObject = null;

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
