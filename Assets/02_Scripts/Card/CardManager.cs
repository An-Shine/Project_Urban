using System.Collections.Generic;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    public static new CardManager Instance { get; private set; }
    [SerializeField] private CardData cardData;

    //임시덱
    public List<CardName> myDeck = new List<CardName>();

    private void Awake()
    {
        // 1. 싱글톤 
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 2. 덱 설정 (Punch 5장, Guard 5장)
        myDeck.Clear();
        for (int i = 0; i < 10; i++) myDeck.Add(CardName.Guard);
        for (int i = 0; i < 10; i++) myDeck.Add(CardName.Punch);

    }

    public Card CreateCard(CardName cardName, Vector3 spawnPos, Transform hand)
    {
        Card prefab = cardData.GetCardPrefab(cardName);
        return Instantiate(prefab, spawnPos, Quaternion.identity, hand);
    }

    public Sprite GetCardSprite(CardName cardName)
    {
        return cardData.GetCardSprite(cardName);
    }

    public Card GetCardPrefab(CardName cardName)
    {
        return cardData.GetCardPrefab(cardName);
    }

    public List<CardName> GetAllCardNames()
    {
        //전체카드목록 호출용
        return cardData.GetAllCardNames();
    }

    public List<CardDataEntry> GetCardsByElement(Element element)
    {
        return cardData.GetCardsByElement(element);
    }       
    
    // 덱 리스트 반환
    public List<CardName> GetMyDeck()
    {
        return myDeck;
    }
}
