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

    [SerializeField] TalkManager talkManager;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI dialogueText;

    [SerializeField] InputField inputField;

    [SerializeField] GameObject _customerPrefab;
    [SerializeField] GameObject _buyerPrefab;
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
    private Coroutine currentDialogueCoroutine;

    private void Awake()
    {
        startBuyItem();
    }

    private void Update()
    {
        wallet.text = money.ToString();
    }

    public void resetDialogue()
    {
        dialogueBox.SetActive(false);
        talkManager.reset();
    }

    public void GetTestTalk()
    {
        string name;
        string talk;


        if (talkManager.GetGreetingTalk(out name, out talk))
        {
            Debug.Log("send from " + name);
            dialogueText.text = talk;
        }
        else
        {
            resetDialogue();
        }

    }

    private void OnGreetingTalk()
    {
        dialogueBox.SetActive(true);

        GetTestTalk();
    }

    private void onTalk(float delay, string talk)
    {
        if (currentDialogueCoroutine != null)
        {
            StopCoroutine(currentDialogueCoroutine);
        }
        currentDialogueCoroutine = StartCoroutine(onTalkEnumerator(delay, talk));
    }

    private IEnumerator onTalkEnumerator(float delay, string talk)
    {
        dialogueBox.SetActive(true);
        dialogueText.text = talk;

        yield return new WaitForSeconds(delay);

        currentDialogueCoroutine = null;
        resetDialogue();
    }

    private void CreateCutomerObject()
    {
        instantiatedCustomerObject = Instantiate(_customerPrefab, cutomerSpwaner.transform.position, Quaternion.identity);

        Invoke("OnItemButtonActive", 1.8f);

    }

    public void OnItemButtonActive()
    {
        itemButton.SetActive(true);

        OnGreetingTalk();
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

        string name;
        string talk;

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
            talkManager.GetTalk(0, 0, 1, out name, out talk);

            onTalk(2.5f, talk);

            money -= suggestedMoney;

            money -= customer.itemData.value;
            player.GetComponent<StorageHolder>().getStorageSystem().AddItem(customer.itemData);



            Invoke("CallNextSeller", 1.5f);

        }
        else
        {
            talkManager.GetTalk(0, 0, 2, out name, out talk);

            onTalk(2.5f, talk); //가격 맘에 안듦
        }

    }

    public void SkipCustomer()
    {
        string name;
        string talk;

        CallNextSeller();


        talkManager.GetTalk(1, 0, 0, out name, out talk);
        onTalk(1f, talk); //구매 포기
    }



    public void CallNextSeller()
    {
        Animator animator = instantiatedCustomerObject.GetComponent<Animator>();

        sellerCount -= 1;

        itemButton.SetActive(false);
        animator.SetBool("exit", true);

        // Invoke("resetDialogue", 1f);


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

    private void CreateBuyerObject()
    {
        instantiatedCustomerObject = Instantiate(_buyerPrefab, cutomerSpwaner.transform.position, Quaternion.identity);

        Invoke("OnItemButtonActive", 1.8f);
    }

    private void startSellItem()
    {
        state = State.sell;

        stateTextObject.text = "판매단계";

        buyerCount = 5;

        CreateBuyerObject();
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
            CallNextSeller();
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
