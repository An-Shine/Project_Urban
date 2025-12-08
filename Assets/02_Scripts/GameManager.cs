using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Player player;
    private int coin;

    public Player Player => player;

    public List<CardName> SelectedBonusCards { get; private set; } = new List<CardName>();

    // 카드를 저장하는 함수
    public void SetBonusCards(CardName card1, CardName card2)
    {
        SelectedBonusCards.Clear();
        SelectedBonusCards.Add(card1);
        SelectedBonusCards.Add(card2);
    }

    public void AddCoin(int amount)
    {
        coin += amount;
    }
}