using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/AllCardData", fileName = "AllCardData")]
public class AllCardData : ScriptableObject
{
    [SerializeField] private List<CardDataEntry> cards = new();

    public Sprite GetCardSprite(CardName cardName)
    {
        CardDataEntry entry = cards.Find(c => c.cardName == cardName);
        return entry?.cardSprite;
    }

    public CardDataEntry GetCardData(CardName cardName)
    {
        return cards.Find(c => c.cardName == cardName);
    }
}

[Serializable]
public class CardDataEntry
{
    public CardName cardName;
    public Sprite cardSprite;
    [TextArea] public string description;
}
