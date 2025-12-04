using System.Collections.Generic;
using UnityEngine;

public class CardFactory : Singleton<CardFactory>
{
    private readonly Dictionary<CardName, Card> cardPrefabMap = new();

    [SerializeField] private List<Card> cardPrefabList = new();

    private void Start()
    {
        foreach (var card in cardPrefabList)
        {
            if (!cardPrefabMap.ContainsKey(card.Name))
            {
                cardPrefabMap.Add(card.Name, card);
            }
        }
    }

    public Card CreateCard(CardName cardName, Vector3 spawnPos, Transform hand)
    {
        return Instantiate(cardPrefabMap[cardName], spawnPos, Quaternion.identity, hand);
    }
}
