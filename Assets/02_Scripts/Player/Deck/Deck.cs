using UnityEngine;
using System.Collections.Generic;

public class Deck 
{
    private readonly List<CardName> originCardList = new();     // 원본 덱 
    private readonly List<CardName> unusedCardList = new();     // 뽑을 덱 
    private readonly List<CardName> usedCardList = new();       // 사용한 카드리스트
    private readonly List<CardName> tempCardList = new(12);         // 카드 임시 버퍼

    private Hand hand = null;
    
    public int UnusedCardCount => unusedCardList.Count;
    public int UsedCardCount => usedCardList.Count;

    public Hand Hand
    {
        get
        {
            hand = hand != null ? hand : BattleManager.Instance.Hand;
            return hand;
        }
    }

    public Deck(IEnumerable<CardName> cardRecipes)
    {
        foreach (CardName name in cardRecipes)
        {
            originCardList.Add(name);
            unusedCardList.Add(name);
        }

        Shuffle();
    }


    public void Shuffle()
    {
        if (usedCardList.Count > 0)
        {
            unusedCardList.AddRange(usedCardList);
            usedCardList.Clear();
        }

        for (int i = unusedCardList.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (unusedCardList[rand], unusedCardList[i]) = (unusedCardList[i], unusedCardList[rand]);
        }
    }

    public void Draw(int amount)
    {
        tempCardList.Clear();
        
        for (int i = 0; i < amount; i++)
            tempCardList.Add(GetNextCardName());

        Hand.AddCards(tempCardList);
    }

    public void Draw()
    {
        Hand.AddCard(GetNextCardName());
    }

    private CardName GetNextCardName()
    {
        if (unusedCardList.Count == 0)
            Shuffle();

        CardName cardName = unusedCardList[^1];
        unusedCardList.RemoveAt(unusedCardList.Count - 1);

        return cardName;
    }

    // 사용한 카드 UsedCardList 로 보내기
    public void Discard(Card usedCard)
    {
        usedCardList.Add(usedCard.Name);
        Hand.RemoveCard(usedCard);
    }

    public void DiscardAll()
    {
        foreach (Card card in Hand.CurHand)
        {
            usedCardList.Add(card.Name);
        }

        Hand.RemoveAll();
    }

    public void ResetDeck()
    {
        unusedCardList.Clear();
        usedCardList.Clear();

        foreach (var cardName in originCardList)
            unusedCardList.Add(cardName);

        Shuffle();
    }

    // 카드 추가
    public void AddCard(CardName cardName)
    {
        originCardList.Add(cardName);
    }

    // 카드 제거
    public void RemoveCard(CardName cardName)
    {
        if (originCardList.Contains(cardName))
            originCardList.Remove(cardName);
    }
}