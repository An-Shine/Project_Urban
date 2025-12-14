using System.Collections.Generic;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    public static new CardManager Instance { get; private set; }
    [SerializeField] private CardData cardData;        

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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


}

