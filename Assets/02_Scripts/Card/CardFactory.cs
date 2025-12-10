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

    public Card GetCardPrefab(CardName cardName)
    {
        return cardPrefabMap[cardName];
    }

    public List<CardName> GetCardsByElement(Element element)
    {
        List<CardName> resultList = new();
        List<CardName> allCards = new(cardPrefabMap.Keys); // 전체 덱

        // 1. 범위 설정
        int minId = 0;
        int maxId = 99;

        switch (element)
        {
            case Element.Flame: minId = 100; maxId = 199; break;
            case Element.Ice:   minId = 200; maxId = 299; break;
            case Element.Grass: minId = 300; maxId = 399; break;
        }

        // 2. 필터링
        foreach (CardName name in allCards)
        {
            int id = (int)name;
            if (id >= minId && id <= maxId)
            {
                resultList.Add(name);
            }
        }
        return resultList;
    }
    // 전체 카드중 랜덤으로 중복없이 N장 뽑는 함수(상점용)
    public List<Card> GetRandomCards(int count)
    {
        List<Card> result = new List<Card>();
        List<Card> pool = new List<Card>(cardPrefabList);

        for (int i = 0; i < count; i++)
        {
            if (pool.Count == 0) break;

            int randomIndex = Random.Range(0, pool.Count);
            result.Add(pool[randomIndex]);
            
            // 중복 방지
            pool.RemoveAt(randomIndex);
        }

        return result;
    }    
    
}
