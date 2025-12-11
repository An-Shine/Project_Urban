using System.Collections.Generic;
using UnityEngine;
public class CardSelectUI : MonoBehaviour
{
    [SerializeField] private List<CardSelectItem> cardSelectItems;

    private void Awake()
    {
        foreach (CardSelectItem item in cardSelectItems)
            item.SetCardInfo(CardName.Strike);
    }
} 