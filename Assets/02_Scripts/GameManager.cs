using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Player player;
    [SerializeField] private List<Card> allCardPrefabs = new List<Card>();
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

    //플레이어가 처음에 선택한 속성 저장(StoreUI 용)
    public Element SelectedElement { get; set; } = Element.None;

    // 메인화면으로 돌아갈 때, 맵 선택창을 띄울지 여부
    public bool IsStageSelectMode { get; set; } = false;

    //선택한 속성의 카드 랜덤뽑기 로직 (UI에서 프리펩 불러오기용)
    public List<Card> GetRandomCardsByElement(Element element, int count)
    {
        List<Card> targetPool = new List<Card>();
        List<Card> result = new List<Card>();

        // 1. 속성에 맞는 ID 범위 설정
        int minId = 0, maxId = 99;
        switch (element)
        {
            case Element.Flame: minId = 100; maxId = 199; break;
            case Element.Ice:   minId = 200; maxId = 299; break;
            case Element.Grass: minId = 300; maxId = 399; break;
        }

        // 전체 카드리스트에서 해당 속성 카드만 추려내기
        foreach (var card in allCardPrefabs)
        {
            if (card == null) continue;
            int id = (int)card.Name;
            if (id >= minId && id <= maxId)
            {
                targetPool.Add(card);
            }
        }

        // 랜덤 뽑기 (중복 방지)
        if (targetPool.Count == 0) return result;
        

        for (int i = 0; i < count; i++)
        {
            if (targetPool.Count == 0) break;

            int randomIndex = Random.Range(0, targetPool.Count);
            result.Add(targetPool[randomIndex]);
            targetPool.RemoveAt(randomIndex); // 뽑은 건 리스트에서 뺌
        }

        return result;
    }

    public void AddCoin(int amount)
    {
        coin += amount;
    }
}