using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ElementWeight
{
    public Element element;
    public float weight = 1f; // 가중치
}


public class CardSelectUI : MonoBehaviour
{
    [SerializeField] private List<CardSelectItem> cardSelectItems;
    [SerializeField] private List<ElementWeight> elementWeights = new()
    {
        new() { element = Element.None },
        new() { element = Element.Flame },
        new() { element = Element.Ice },
        new() { element = Element.Grass },
    };

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

        var selectedCards = SelectRandomCards(cardSelectItems.Count);
        
        int index = 0;
        foreach (var item in cardSelectItems)
        {
            item.SetCardInfo(selectedCards[index++]);
        }
    }

    private List<CardName> SelectRandomCards(int count)
    {
        List<CardName> result = new();
        
        for (int i = 0; i < count; i++)
        {
            Element selectedElement = SelectRandomElement();
            CardName randomCard = GetRandomCardFromElement(selectedElement);
            result.Add(randomCard);
        }
        
        return result;
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

    private CardName GetRandomCardFromElement(Element element)
    {
        var cardNames = CardManager.Instance.GetCardsByElement(element).Keys.ToList();
        return cardNames[Random.Range(0, cardNames.Count)];
    }
} 