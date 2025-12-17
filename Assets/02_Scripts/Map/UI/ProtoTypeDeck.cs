using UnityEngine;
using System.Collections.Generic;

public class ProtoTypeDeck : MonoBehaviour
{
    public static ProtoTypeDeck Instance;
    public List<CardDataEntry> deck;

    [System.Serializable]
    public struct DeckRecipe
    {
        public CardName cardName;
        public int count;
    }

    [Header("카드 데이터베이스")]
    [SerializeField] private CardData cardDatabase;

    [Header("임시 덱 구성")]
    [SerializeField] private List<DeckRecipe> deckConfig = new List<DeckRecipe>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public List<CardDataEntry> GetCurrentDeck()
    {
        List<CardDataEntry> generatedDeck = new List<CardDataEntry>();

        foreach (DeckRecipe recipe in deckConfig)
        {
            CardDataEntry entry = cardDatabase.GetCardData(recipe.cardName);

            // 설정한 개수만큼 리스트에 추가
            for (int i = 0; i < recipe.count; i++)
            {
                generatedDeck.Add(entry);
            }
        }

        return generatedDeck;
    }

    public void RemoveCard(CardDataEntry card)
{
    if (deck.Contains(card))
    {
        deck.Remove(card);
    }   
}

}