using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class StoreUI : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private Transform contentArea;  
    [SerializeField] private GameObject uiCardPrefab;   // UI_Card 프리팹
    [SerializeField] private BuyCardPopup buyCardPopup; 
    [SerializeField] private TMP_Text currentCoinText; 

    // 데이터 관리 리스트
    private List<UICard> _spawnedCards = new List<UICard>(); 
    private List<CardDataEntry> _shopData = new List<CardDataEntry>(); 
    private List<int> _shopPrices = new List<int>(); 
    private List<bool> _isSoldOut = new List<bool>();
    private CardName[] currentCardsInSlots = new CardName[6];

    private void Start()
    {
        SetupStore();
        UpdateCoinUI();
    }

    private void Update()
    {
        // 현재 보유중 코인표시
        currentCoinText.text = $"{GameManager.Instance.Coin}";
    }

    public void SetupStore()
    {
        // 1. 기존 슬롯 삭제
        foreach (Transform child in contentArea) Destroy(child.gameObject);
        
        _spawnedCards.Clear();
        _shopData.Clear();
        _shopPrices.Clear();
        _isSoldOut.Clear();

        // 2. 카드 목록 가져오기
        List<CardName> allNames = CardManager.Instance.GetAllCardNames();
        List<CardName> deckToDraw = new List<CardName>(allNames);

        // 3. 6개 생성 루프
        for (int i = 0; i < 6; i++)
        {
            if (deckToDraw.Count == 0) break;

            // 랜덤 뽑기
            int randIndex = Random.Range(0, deckToDraw.Count);
            CardName pickedName = deckToDraw[randIndex];
            deckToDraw.RemoveAt(randIndex);

            // 데이터 가져오기
            CardDataEntry cardData = CardManager.Instance.GetCardData(pickedName);

            // 가격 설정 (나중에 cardData.price 등으로 변경 예정)
            int price = 100; 

            // 리스트 저장
            _shopData.Add(cardData);
            _shopPrices.Add(price);
            _isSoldOut.Add(false);

            // 프리팹 생성 및 컴포넌트 가져오기
            GameObject newObj = Instantiate(uiCardPrefab, contentArea);
            UICard uiCard = newObj.GetComponent<UICard>();
            _spawnedCards.Add(uiCard);

            // 1. 카드 정보 + 클릭 함수(인덱스 전달)
            int index = i; 
            uiCard.SetCardDataEntry(cardData, (data) => OnClickSlot(index));
            
            // 2. 가격 표시
            uiCard.SetStoreMode(price);
        }
    }

    // 슬롯 클릭 시
    public void OnClickSlot(int index)
    {
        if (_isSoldOut[index]) return;

        int price = _shopPrices[index];

        // 돈 확인
        if (GameManager.Instance.Coin >= price)
        { 
            // 팝업 열기
            CardDataEntry data = _shopData[index];
            buyCardPopup.OpenPopup(this, index, data);
        }
    }

    public void OnClickBuy(int index)
    {
        if (_isSoldOut[index]) return;

        int price = 100; 

        if (GameManager.Instance.Coin >= price)
        {
            // 1. 이름 가져오기
            CardName cName = currentCardsInSlots[index];

            // 2. 이름으로 카드 데이터 전체(CardDataEntry) 찾기           
            CardDataEntry cardData = CardManager.Instance.GetCardData(cName); 

            // 3. 팝업에 데이터 통째로 전달
            buyCardPopup.OpenPopup(this, index, cardData);
        }       
    }

    // 구매 확정
    public void ConfirmPurchase(int index)
    {
        int price = _shopPrices[index];

        // 코인 사용처리, 카드추가
        GameManager.Instance.AddCoin(-price);
        GameManager.Instance.Deck.AddCard(_shopData[index].cardName);

        // 품절 처리
        _isSoldOut[index] = true;
        _spawnedCards[index].SetSoldOut(); // UI 갱신

        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        currentCoinText.text = $"Coin: {GameManager.Instance.Coin}";
    }
}