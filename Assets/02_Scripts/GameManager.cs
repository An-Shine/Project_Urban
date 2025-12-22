using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CardRecipe
{
    public CardName name;
    public int count;

}

public class GameManager : Singleton<GameManager>
{
    [Header("Initial Deck Recipe")]
    [SerializeField] private List<CardRecipe> initialDeckRecipe = new();

    [Header("Player Hp Settings")]
    [SerializeField] private int playerMaxHp = 500;
    private readonly List<CardName> initialDeck = new();

    private HealthController playerHp;
    private CoinController coin;
    private Deck deck;

    public HealthController PlayerHealth => playerHp;
    public CoinController Coin => coin;
    public Deck Deck => deck;

    private void Start()
    {
        playerHp = new HealthController(playerMaxHp);
        coin = new CoinController();
    }

    // 카드를 저장하는 함수
    public void SetBonusCards(IEnumerable<CardName> cardNames)
    {
        initialDeck.AddRange(cardNames);

        foreach (CardRecipe recipe in initialDeckRecipe)
        {
            for (int i = 0; i < recipe.count; i++)
                initialDeck.Add(recipe.name);
        }

        deck = new(initialDeck);
    }

    //플레이어가 처음에 선택한 속성 저장(StoreUI 용)
    public Element SelectedElement { get; set; } = Element.None;

    // 메인화면으로 돌아갈 때, 맵 선택창을 띄울지 여부
    // public bool IsStageSelectMode { get; set; } = false;

    public void AddCoin(int amount)
    {
        coin.IncreaseCoin(amount);
    }

    public void UseCoin(int amount)
    {
        coin.DecreaseCoin(amount);
    }
}