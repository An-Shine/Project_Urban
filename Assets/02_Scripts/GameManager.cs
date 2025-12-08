using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    private int coin;
    public List<CardName> SelectedBonusCards { get; private set; } = new List<CardName>();

    // 카드를 저장하는 함수
    public void SetBonusCards(CardName card1, CardName card2)
    {
        SelectedBonusCards.Clear();
        SelectedBonusCards.Add(card1);
        SelectedBonusCards.Add(card2);
    }

    //플레이어가 처음에 선택한 속성 저장(StoreUI 용)
    public Element SelectedElement { get; set; } = Element.None;

    // 메인화면으로 돌아갈 때, 맵 선택창을 띄울지 여부
    public bool IsStageSelectMode { get; set; } = false;

    public void AddCoin(int amount)
    {
        coin += amount;
    }
}