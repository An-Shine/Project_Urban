using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class StoreUI : MonoBehaviour
{
    [SerializeField] private Deck deck;
    [SerializeField] private StoreCardList[] slots;

    [SerializeField] private int cardPrice = 100;       //카드 비용
    [SerializeField] private int rerollCost = 50;       //리롤 비용

    private List<CardName> availableCardPool = new List<CardName>();

    private void Start()
    {   
        //Player 에서 Deck 찾기
        if(deck == null)
        {
            var player = FindFirstObjectByType<Player>();
            if(player != null) 
            {
                deck = player.GetComponent<Deck>();               
            }            

        if(deck != null)
        {    
            InitStore();        //초기화
        }
    }
}

    private void InitStore()
    {
        // 1. 속성에 맞는 카드풀 생성        
        CreateCardPool();

        // 2. 각 슬롯에 랜덤 카드 배치
        foreach(var slot in slots)
        {
            if(slot != null)SetRandomCardToSlot(slot);
        }
    }

    // 현재 선택된 속성에 따라 등장할 카드리스트를 작성하는 함수
    private void CreateCardPool()
    {
        availableCardPool.Clear();
        if (deck == null) return;

        // 1. 현재 속성 확인
        Element currentElement = Element.None;
        if (GameManager.Instance != null)
            currentElement = GameManager.Instance.SelectedElement;

        // 2. Deck의 공용 함수
        var cards = CardManager.Instance.GetCardsByElement(currentElement);     
        availableCardPool = cards.Select(entry => entry.cardName).ToList();   
    }

    // 슬롯에 랜덤카드배치 함수
    public void SetRandomCardToSlot(StoreCardList slot)
    {
        if(availableCardPool.Count == 0) return;  

        // 1. 카드이름 리스트에서 랜덤선택
        int randomIndex = Random.Range(0, availableCardPool.Count);
        CardName pickedName = availableCardPool[randomIndex];

        // 2. Deck 에서 카드 프리펩 가져오기
        Card prefab = CardManager.Instance.GetCardPrefab(pickedName);

        // 3. 슬롯 UI 세팅
        if(prefab != null)
        {
            slot.StoreSetting(this, prefab, cardPrice);           
        }
        
    }

    // 카드구매 함수
    public void BuyCard(CardName cardName, int price, StoreCardList slot)
    {
        //TODO : 카드 구매 필요골드 확인로직 필요함
        deck.AddCard(cardName);
        slot.SetSoldOut();
    }

    // 슬롯 리롤 함수
    public void RerollSlot(StoreCardList slot)
    {
        //TODO : 리롤 필요골드 확인로직 필요함
        SetRandomCardToSlot(slot);
    }
}