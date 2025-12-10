using UnityEngine;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    [System.Serializable]
    struct CardRecipe
    {
        public CardName name;
        public int count;

    }
    // 초기 덱 레시피 
    [Header("Initial Deck Recipe")]
    [SerializeField] private List<CardRecipe> initialDeckRecipe = new();

    //핸드 설정
    [Header("Hand Object")]
    [SerializeField] private Hand hand;

    private readonly List<CardName> originCardList = new();     // 원본 덱 
    private readonly List<CardName> unusedCardList = new();     // 뽑을 덱 
    private readonly List<CardName> usedCardList = new();       // 사용한 카드리스트
    private readonly List<CardName> tempCardList = new(12);         // 카드 임시 버퍼    

    public int UnusedCardCount => unusedCardList.Count;
    public int UsedCardCount => usedCardList.Count;
    public int OriginCount => originCardList.Count; //상점에서 덱 장수 확인용

    private void Awake()
    {
        // 1. GameManager가 있고, 저장된 전역 덱(GlobalDeck)이 있는 경우 -> 그거 가져옴
        if (GameManager.Instance != null && GameManager.Instance.GlobalDeck.Count > 0)
        {
            originCardList.Clear();
            
            // ★ 중요: 리스트를 그대로 가져오면 안되고 '복사'해야 합니다.
            // (배틀 중에 소멸/변화가 원본에 영향을 주지 않도록)
            originCardList.AddRange(GameManager.Instance.GlobalDeck);
            
            Debug.Log($" GameManager에서 덱 로드 완료! ({originCardList.Count}장)");
        }
        else
        {
            // 2. 저장된 덱이 없는 경우 (첫 시작 or 에디터 테스트) -> 초기 레시피로 생성
            DeckMaking();

            // 처음 만든 이 덱을 GameManager에 '원본'으로 등록해둠
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetInitialDeck(originCardList);
                Debug.Log(" 초기 덱을 GameManager에 등록했습니다.");
            }
        }

        foreach (var cardName in originCardList)
            unusedCardList.Add(cardName);

        Shuffle();
    }

    // 덱 레시피 -> originCardList 변환 함수
    private void DeckMaking()
    {
        originCardList.Clear();

        foreach (var recipe in initialDeckRecipe)
        {
            for (int i = 0; i < recipe.count; i++)
            {
                originCardList.Add(recipe.name);
            }
        }
        Debug.Log($"[Deck] 초기 덱 구성 완료. 총 {originCardList.Count}장의 카드가 로드되었습니다.");

        // 데모때는 주석 풀기
        // 초기 속성카드
        foreach (var cardName in GameManager.Instance.SelectedBonusCards)
        {
            originCardList.Add(cardName);
        }
        Debug.Log($"[Deck] 속성 보너스 카드 추가됨! (현재 총 {originCardList.Count}장)");
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

        hand.AddCards(tempCardList);
    }

    public void Draw()
    {
        hand.AddCard(GetNextCardName());
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
        hand.RemoveCard(usedCard);
    }

    public void DiscardAll()
    {
        foreach (Card card in hand.CurHand)
        {
            usedCardList.Add(card.Name);
        }

        hand.RemoveAll();
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
        Debug.Log($" '{cardName}' 카드가 추가되었습니다.");
        Debug.Log($" 총 카드 개수: {originCardList.Count}장");
    }

    //상점에서 구매한 카드 추가용
    public void AddCard(Card card)
    {
        Debug.Log($"🔍 [4단계] Deck.AddCard(Card객체) 도착! 카드이름: {card.Name}");
        AddCard(card.Name); 
    }

    // 카드 제거
    public void RemoveCard(CardName cardName)
    {
        if (originCardList.Contains(cardName))
            originCardList.Remove(cardName);
    }
}