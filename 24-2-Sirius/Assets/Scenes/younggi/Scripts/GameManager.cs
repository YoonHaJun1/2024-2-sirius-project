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

    [SerializeField] GameObject tradeObject;
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

    private bool isGreetingEnd;

    private GameObject instantiatedCustomerObject;
    private GameObject instantiatedBuyerObject;
    private Animator animator;
    private Coroutine currentDialogueCoroutine;
    private GameObject buyer;

    private void Awake()
    {
        startBuyItem();
    }

    private void Update()
    {
        wallet.text = money.ToString();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueBox.activeSelf == true)
            {
                OnGreetingTalk();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (dialogueBox.activeSelf == true)
            {
                OnGreetingTalk();
            }
            else if (tradeObject.activeSelf == true)
            {
                SuggestMoney();
            }
        }

    }

    private void resetDialogue()
    {
        dialogueBox.SetActive(false);
        talkManager.reset();
    }

    public void GetBuyLevelGreetingTalk()
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
            isGreetingEnd = true;
            resetDialogue();
        }

    }

    private void OnGreetingTalk()
    {
        if (state == State.buy)
        {
            if (!isGreetingEnd)
            {
                dialogueBox.SetActive(true);

                GetBuyLevelGreetingTalk();
            }
        }

    }

    private void onTalk(float delay, string talk)
    {
        isGreetingEnd = true;

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
        isGreetingEnd = false;

        instantiatedCustomerObject = Instantiate(_customerPrefab, cutomerSpwaner.transform.position, Quaternion.identity);

        Invoke("OnItemButtonActive", 1.8f);
    }

    private void OnItemButtonActive()
    {
        itemButton.SetActive(true);

        OnGreetingTalk();
    }

    private void startBuyItem()
    {
        state = State.buy;

        round += 1;
        sellerCount = 3;

        CreateCutomerObject();

        stateTextObject.text = "구매단계";
    }


    public void SuggestMoney() //흥정 입력 값 저장 및 지갑에서 돈 빼기
    {
        if (state == State.buy)
        {
            SuggestBuy();
        }
        if (state == State.sell)
        {
            SuggestSell();
        }

    }

    private void SuggestBuy()
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

        if (suggestedMoney >= customer.itemData.value - customer.difficultyLevel) //만약 받아주면
        {
            talkManager.GetTalk(0, 0, 1, out name, out talk);

            onTalk(2.5f, talk);

            money -= suggestedMoney;
            player.GetComponent<StorageHolder>().getStorageSystem().AddItem(customer.itemData);

            Invoke("CallNextSeller", 1.5f);
        }
        else
        {
            if (customer.patienceLevel == 0)
            {
                Debug.Log("더이상 흥정을 받아들이지 않겠네");
                Invoke("CallNextSeller", 1.5f);

            }
            else if (suggestedMoney <= customer.itemData.value * 0.2)
            {
                Debug.Log("이런 말도 안되는 가격을 제시하다니");
                Invoke("CallNextSeller", 1.5f);
            }
            else
            {
                talkManager.GetTalk(0, 0, 2, out name, out talk);
                customer.patienceLevel -= 1;
                onTalk(2.5f, talk); //가격 맘에 안듦
            }   
        }
    }

    private void SuggestSell()
    {
        int suggestedMoney = 0; //흥정 입력값 (int)
        string input = inputField.text; //흥정 요구 값 저장
        Buyer buyerComponent = instantiatedBuyerObject.GetComponent<Buyer>();
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

        if (suggestedMoney <= buyerComponent.selectedItem.value * buyerComponent.profitRatio) //만약 받아주면
        {
            Debug.Log("좋아요");

            money += suggestedMoney;

            player.GetComponent<StorageHolder>().getStorageSystem().RemoveItem(buyerComponent.selectedItem);

            Invoke("CallNextBuyer", 1.5f);
        }
        else
        {
            if (buyerComponent.patienceLevel == 0)
            {
                Debug.Log("더이상 흥정을 받아들이지 않겠네");
                Invoke("CallNextBuyer", 1.5f);
            }
            else if (suggestedMoney >= buyerComponent.selectedItem.value * 2.5)
            {
                Debug.Log("이런 말도 안되는 가격을 제시하다니");
                Invoke("CallNextBuyer", 1.5f);
            }
            else
            {
                talkManager.GetTalk(0, 0, 2, out name, out talk);
                buyerComponent.Bargain(suggestedMoney);
                buyerComponent.patienceLevel -= 1;
                onTalk(2.5f, talk); //가격 맘에 안듦
            }   
        }
    }

    public void SetBuyer(GameObject buyerObject)
    {
        buyer = buyerObject;
    }

    public void SkipCustomer()
    {
        string name;
        string talk;

        CallNextSeller();


        talkManager.GetTalk(1, 0, 0, out name, out talk);
        onTalk(1f, talk); //구매 포기
    }


    private void CallNextSeller()
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

    private void CallNextBuyer()
    {
        Animator animator = instantiatedBuyerObject.GetComponent<Animator>();

        buyerCount -= 1;

        itemButton.SetActive(false);
        animator.SetBool("exit", true);

        if (buyerCount > 0)
        {
            Invoke("CreateBuyerObject", 4f);
        }
        else
        {
            instantiatedBuyerObject = null;

            Invoke("turnEnd", 4f);
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
        instantiatedBuyerObject = Instantiate(_buyerPrefab, cutomerSpwaner.transform.position, Quaternion.identity);
        
        Buyer buyer = instantiatedBuyerObject.GetComponent<Buyer>();
    
        if(buyer.selectedItem == null){
            Debug.Log("살게 없네");
            Invoke("CallNextBuyer", 1.5f);
        }else{
            Invoke("OnItemButtonActive", 1.8f);
        }
        
    }

    private void startSellItem()
    {
        state = State.sell;

        stateTextObject.text = "판매단계";

        buyerCount = 3;

        CreateBuyerObject();
    }

    private void turnEnd()
    {
        state = State.result;

        stateTextObject.text = "결산";

        if(round == 4){
            Debug.Log("돈 내");
        }
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
            CallNextBuyer();
        }
        else if (state == State.result)
        {
            Debug.Log("end!!!!!!");
            startBuyItem();
        }
    }
}
