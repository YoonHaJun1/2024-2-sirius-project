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
    [SerializeField] GameObject itemButton;


    private int round = 0;
    private int money = 10000;
    private int sellerCount = 0;
    private int buyerCount = 0;
    private int itemCount = 0;
    private int preItemCount = 0;

    private GameObject instantiatedCustomerObject;
    private Animator animator;

    private void Awake()
    {
        startBuyItem();
    }

    private void Update()
    {
        wallet.text = money.ToString();
    }

    private void CreateCutomerObject()
    {
        instantiatedCustomerObject = Instantiate(_customerPrefab, cutomerSpwaner.transform.position, Quaternion.identity);

        Invoke("OnItemButtonActive", 1.8f);

    }

    public void OnItemButtonActive()
    {
        itemButton.SetActive(true);
    }

    private void startBuyItem()
    {
        state = State.buy;

        round += 1;
        sellerCount = 5;

        CreateCutomerObject();

        stateTextObject.text = "구매단계";
    }


    public void SuggestMoney() //흥정 입력 값 저장 및 지갑에서 돈 빼기
    {
        int suggestedMoney = 0; //흥정 입력값 (int)
        string input = inputField.text; //흥정 요구 값 저장

        if (!string.IsNullOrWhiteSpace(input))
        {
            int.TryParse(input, out suggestedMoney); //string -> suggestedMoney int 값으로 변환
        }
        else
        {
            Debug.Log("제대로 제시해!!");
            return;
        }

        if (!player.GetComponent<StorageHolder>().getStorageSystem().HasSlot())
        {
            Debug.Log("자리가 없어!!!");
            return;
        }

        Customer customer = instantiatedCustomerObject.GetComponent<Customer>();


        if (suggestedMoney >= customer.itemData.value - customer.patienceLevel) //만약 받아주면
        {
            Debug.Log("좋아용");
            money -= suggestedMoney;

            money -= customer.itemData.value;
            player.GetComponent<StorageHolder>().getStorageSystem().AddItem(customer.itemData);

            CallNextBuyer();
        }
        else
        {
            Debug.Log("그 가격은 안되지");
        }

    }


    public void CallNextBuyer()
    {
        Animator animator = instantiatedCustomerObject.GetComponent<Animator>();

        sellerCount -= 1;

        itemButton.SetActive(false);
        animator.SetBool("exit", true);

        if (sellerCount > 0)
        {

            Invoke("CreateCutomerObject", 4f);
        }
        else
        {
            instantiatedCustomerObject = null;

            Invoke("startMagicItem", 4f);
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
            CallNextBuyer();
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
