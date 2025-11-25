using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{   
    [System.Serializable]
    public struct DeckRecipe
    {
        public CardType type; 
        public int count;     
    }

    [System.Serializable]
    public struct CardPrefabMapping
    {
        public CardType type;
        public Card prefab;   // 연결할 프리팹
    }
    // 초기 덱 레시피 
    public List<DeckRecipe> initialDeckRecipe = new List<DeckRecipe>();

    // 카드 프리팹 도감 
    public List<CardPrefabMapping> cardPrefabs = new List<CardPrefabMapping>();

    // 덱 딕셔너리
    private Dictionary<CardType, Card> prefabDict = new Dictionary<CardType, Card>();
    
    
    // 게임 데이터 
    // 원본 덱 
    [SerializeField] private List<Card> originCardList = new List<Card>();

    // 뽑을 덱 
    [SerializeField] private List<Card> unusedCardList = new List<Card>();

    // 사용한 카드리스트
    [SerializeField] private List<Card> usedCardList = new List<Card>();

   
    public int UnusedCardCount => unusedCardList.Count;
    public int UsedCardCount => usedCardList.Count;

  

    private void Awake()
    {
        // 1. 프리팹 리스트를 딕셔너리로 변환
        prefabDict.Clear();
        foreach (var mapping in cardPrefabs)
        {
            if (!prefabDict.ContainsKey(mapping.type))
            {
                prefabDict.Add(mapping.type, mapping.prefab);
            }
        }
    }

    private void Start()
    {
        // 2. 레시피(설정)를 보고 원본 덱(OriginCardList)을 만든다 
        DeckRecipe();

        // 3. 게임 시작 상태로 리셋
        ResetDeck();
    }

    // 덱 레시피 -> originCardList 변환 함수
    private void DeckRecipe()
    {
        originCardList.Clear();

        foreach (var recipe in initialDeckRecipe)
        {
            // 딕셔너리에 해당 타입의 프리팹이 있는지 확인
            if (prefabDict.ContainsKey(recipe.type))
            {
                Card prefab = prefabDict[recipe.type];

                // 설정된 수량(count)만큼 프리팹을 원본 리스트에 등록
                for (int i = 0; i < recipe.count; i++)
                {
                    originCardList.Add(prefab);
                }
            }
            else
            {
                Debug.LogWarning($"[Deck] {recipe.type} 타입의 프리팹을 찾을 수 없습니다! Card Prefabs 리스트를 확인하세요.");
            }
        }
        
        Debug.Log($"[Deck] 초기 덱 구성 완료. 총 {originCardList.Count}장의 카드가 로드되었습니다.");
    }    

    // [Shuffle] 즉시 덱을 섞는다
    public void Shuffle()
    {
        // UsedCardList 에 카드가 없으면 섞을 필요 없음
        if (usedCardList.Count == 0 && unusedCardList.Count > 0) return;

        // UsedCardList -> 덱으로 이동
        unusedCardList.AddRange(usedCardList);
        usedCardList.Clear();

        // 랜덤 섞기
        for (int i = unusedCardList.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            Card temp = unusedCardList[i];
            unusedCardList[i] = unusedCardList[rand];
            unusedCardList[rand] = temp;
        }
    }

    // 카드를 n장 뽑는다
    public List<Card> DrawCard(int amount)
    {
        List<Card> drawnCards = new List<Card>();

        for (int i = 0; i < amount; i++)
        {
            // 1. 덱이 비었는지 확인
            if (unusedCardList.Count == 0)
            {
                // 즉시 셔플
                Shuffle();

                // 셔플 후에도 없으면 중단
                if (unusedCardList.Count == 0) break;
            }

            // 2. 맨 뒤의 카드를 가져옴 
            int lastIndex = unusedCardList.Count - 1;
            Card cardPrefabToDraw = unusedCardList[lastIndex];

            // 3. 리스트에서 제거
            unusedCardList.RemoveAt(lastIndex);
            
            // 4. 결과 리스트에 추가            
            drawnCards.Add(cardPrefabToDraw);
        }

        return drawnCards;
    }

    // 원본 리스트를 기반으로 게임 덱 초기화
    public void ResetDeck()
    {
        unusedCardList.Clear();
        usedCardList.Clear();

        // Origin(프리팹 원본들)을 Unused로 복사
        foreach (var card in originCardList)
        {
            unusedCardList.Add(card);
        }

        // 섞기
        Shuffle();
    }

    // 카드 추가
    public void AddCard(Card card)
    {
        if (card == null) return;
        originCardList.Add(card);
        unusedCardList.Add(card);
        Shuffle();
    }

    // 카드 제거
    public void RemoveCard(Card card)
    {
        if (originCardList.Contains(card)) originCardList.Remove(card);
        if (unusedCardList.Contains(card)) unusedCardList.Remove(card);
    }

    // 사용한 카드 UsedCardList 로 보내기
    public void Discard(Card card)
    {        
        usedCardList.Add(card);
    }
}