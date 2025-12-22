using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ElementWeight
{
    public Element element;
    public float weight = 1f; // 가중치
}

public class CardSelectUI : MonoBehaviour
{
    [SerializeField] private List<UICard> cardSelectItems;
    private readonly List<ElementWeight> elementWeights = new()
    {
        new() { element = Element.None },
        new() { element = Element.Flame },
        new() { element = Element.Ice },
        new() { element = Element.Grass },
    };

    public void Initialize(UnityAction<CardName> onCardSelected)
    {
        foreach (var card in cardSelectItems)
        {
            card.AddClickEventHandler(() => onCardSelected(card.CardName));
        }
    }

    private void Awake()
    {
        foreach (ElementWeight elementWeight in elementWeights)
        {
            if (elementWeight.element == GameManager.Instance.SelectedElement)
            {
                elementWeight.weight *= 2.0f;
                break;
            }
        }

        for (int i = 0; i < cardSelectItems.Count; i++)
        {
            Element selectedElement = SelectRandomElement();
            CardDataEntry randomCard  = GetRandomCardFromElement(selectedElement);
            cardSelectItems[i].SetCardDataEntry(randomCard);
        }
    }

    private Element SelectRandomElement()
    {
        float totalWeight = 0f;
        foreach (var ew in elementWeights)
            totalWeight += ew.weight;
        
        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;
        
        foreach (var ew in elementWeights)
        {
            currentWeight += ew.weight;
            if (randomValue <= currentWeight)
                return ew.element;
        }
        
        return elementWeights[0].element;
    }

    private CardDataEntry GetRandomCardFromElement(Element element)
    {
        List<CardDataEntry> cards = CardManager.Instance.GetCardsByElement(element);
        return cards[Random.Range(0, cards.Count)];
    }
} 