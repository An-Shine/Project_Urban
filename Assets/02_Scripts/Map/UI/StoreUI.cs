using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class StoreUI : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private Transform contentArea;  
    [SerializeField] private GameObject uiCardPrefab;   // UI_Card 프리팹
    [SerializeField] private BuyCardPopup buyCardPopup; 
    [SerializeField] private TMP_Text currentCoinText; 
    [SerializeField] private List<TMP_Text> priceText; // 가격 텍스트 (이제 MapManager가 처리하므로 안 쓸 수도 있음)
    [SerializeField] private List<Button> slotButtons; // 이것도 MapManager가 처리하면 필요 없을 수 있음

    // 데이터 관리 리스트
    private List<UICard> _spawnedCards = new List<UICard>(); 
    private List<CardDataEntry> _shopData = new List<CardDataEntry>(); 
    private List<int> _shopPrices = new List<int>(); 
    private List<bool> _isSoldOut = new List<bool>();
    
    // [중요] OnClickBuy에서 사용하려던 배열이 초기화가 안 되어 있어서, _shopData를 활용하는 것으로 통일합니다.
    // private CardName[] currentCardsInSlots = new CardName[6]; 

    private void Start()
    {
        SetupStore();
        UpdateCoinUI();
    }

    private void Update()
    {
        // 현재 보유중 코인표시
        if (GameManager.Instance != null)
        {
            currentCoinText.text = $"{GameManager.Instance.Coin}";
        }
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
            
            int index = i; 

            // 랜덤 뽑기
            int randIndex = Random.Range(0, deckToDraw.Count);
            CardName pickedName = deckToDraw[randIndex];
            deckToDraw.RemoveAt(randIndex);

            // 데이터 가져오기
            CardDataEntry cardData = CardManager.Instance.GetCardData(pickedName);

            // 가격 설정
            int price = cardData.Price; 

            // 리스트 저장
            _shopData.Add(cardData);
            _shopPrices.Add(price);
            _isSoldOut.Add(false);

            // 프리팹 생성 및 컴포넌트 가져오기
            GameObject newObj = Instantiate(uiCardPrefab, contentArea);
            newObj.transform.localScale = Vector3.one;
            
            UICard uiCard = newObj.GetComponent<UICard>();
            uiCard.SetCardDataEntry(cardData);
            uiCard.AddClickEventHandler(() => OnClickSlot(index));
            _spawnedCards.Add(uiCard);
            
            // 클릭 시 실행할 함수 연결

            // MapManager를 통해 상점 모드(가격 표시) 설정
            MapManager.Instance.ConfigureShopCard(uiCard, price);
        }
    }

    // 슬롯(카드) 클릭 시 호출됨
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

    // 구매 팝업에서 "구매" 버튼 눌렀을 때 
    public void OnClickBuy(int index)
    {
        if (_isSoldOut[index]) return;

        int price = _shopPrices[index];

        if (GameManager.Instance.Coin >= price)
        {
            CardDataEntry cardData = _shopData[index];

            // 팝업에 데이터 전달
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
       
        MapManager.Instance.SetCardSoldOut(_spawnedCards[index]);

        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (GameManager.Instance != null)
        {
            currentCoinText.text = $"Coin: {GameManager.Instance.Coin}";
        }
    }
}