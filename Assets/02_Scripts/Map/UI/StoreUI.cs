using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class StoreUI : MonoBehaviour
{
    [SerializeField] private List<Image> slotImages;
    
    [SerializeField] private List<Button> slotButtons;
    
    [SerializeField] private List<TMP_Text> slotPriceTexts;
    
    [SerializeField] private TMP_Text currentCoinText; 
    [SerializeField] private BuyCardPopup buyCardPopup;

    // 현재 각 슬롯에 어떤 카드가 들어있는지 기억하는 배열
    private CardName[] _currentCardsInSlots = new CardName[6];
    private bool[] _isSoldOut = new bool[6]; 

    private void Start()
    {
        // 1. 버튼들에 클릭 이벤트 자동 연결
        for (int i = 0; i < slotButtons.Count; i++)
        {
            int index = i; 
            slotButtons[i].onClick.RemoveAllListeners(); 
            slotButtons[i].onClick.AddListener(() => OnClickBuy(index));
        }

        // 2. 상점 초기화
        if (CardManager.Instance != null)
        {
            SetupStore();
            UpdateCoinUI();
        }
    }

    private void Update()
    {
        if (GameManager.Instance != null && currentCoinText != null)
        {
            currentCoinText.text = $"Coin: {GameManager.Instance.Coin}";
        }
    }

    public void SetupStore()
    {
        if (CardManager.Instance == null) return;

        // 전체 카드 목록 가져와서 복사
        List<CardName> deckToDraw = new List<CardName>(CardManager.Instance.GetAllCardNames());

        for (int i = 0; i < 6; i++)
        {
            if (i >= slotImages.Count || i >= slotButtons.Count || i >= slotPriceTexts.Count) break;
            if (deckToDraw.Count == 0) break;

            // 1. 랜덤 카드 뽑기
            int randIndex = Random.Range(0, deckToDraw.Count);
            CardName pickedCard = deckToDraw[randIndex];
            deckToDraw.RemoveAt(randIndex); 

            // 2. 데이터 저장
            _currentCardsInSlots[i] = pickedCard;
            _isSoldOut[i] = false;

            // 3. UI 세팅
            slotImages[i].sprite = CardManager.Instance.GetCardSprite(pickedCard);
            slotImages[i].color = Color.white; 
            slotImages[i].gameObject.SetActive(true);

            slotPriceTexts[i].text = "100 G";
            slotPriceTexts[i].color = Color.black;
            slotButtons[i].interactable = true;
        }
        // 2. Deck의 공용 함수
        var cards = CardManager.Instance.GetCardsByElement(currentElement);     
        availableCardPool = cards.Select(entry => entry.cardName).ToList();   
    }

    // i번째 버튼을 눌렀을 때 실행되는 함수
    public void OnClickBuy(int index)
    {
        if (_isSoldOut[index]) return;

        int price = 100; 

        // 돈이 있는지 먼저 확인
        if (GameManager.Instance.Coin >= price)
        {
            // 돈이 있으면 팝업창 띄움
            string cardNameStr = _currentCardsInSlots[index].ToString();
            buyCardPopup.OpenPopup(this, index, cardNameStr);
        }       
    }

    public void ConfirmPurchase(int index)
    {
        int price = 100;

        // 1. 구매 및 카드 추가
        GameManager.Instance.AddCoin(-price);
        GameManager.Instance.Deck.AddCard(_currentCardsInSlots[index]); 

        // 2. 품절 처리
        _isSoldOut[index] = true;

        // 3. UI 갱신
        slotPriceTexts[index].text = "Sold Out";        

        slotButtons[index].interactable = false; // 버튼 비활성화
        slotImages[index].color = Color.gray;    // 이미지 회색        
    }

    private void UpdateCoinUI()
    {
        if (GameManager.Instance != null && currentCoinText != null)
            currentCoinText.text = $"Coin: {GameManager.Instance.Coin}";
    }
}