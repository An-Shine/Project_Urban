using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Deck : MonoBehaviour
{
    private readonly Dictionary<CardName, Card> prefabDict = new();
    [System.Serializable]
    struct CardRecipe
    {
        public CardName name;
        public int count;
    }
    // 초기 덱 레시피 
    [SerializeField] private List<CardRecipe> initialDeckRecipe = new();

    // 전체 카드 프리팹
    [SerializeField] private List<Card> cardPrefabList = new();

    //핸드 설정
    [SerializeField] private Transform handParent; // 카드가 생성되는 핸드 오브젝트
    [SerializeField] private float cardSpacing = 2.5f; // 카드 간격

    // 덱 딕셔너리

    // 게임 데이터 
    // 원본 덱 
    private readonly List<CardName> originCardList = new();
    // 뽑을 덱 
    private readonly List<CardName> unusedCardList = new();
    // 사용한 카드리스트
    private readonly List<CardName> usedCardList = new();

    //현재 핸드에 있는 카드리스트
    private readonly List<Card> currentHandList = new();


    public int UnusedCardCount => unusedCardList.Count;
    public int UsedCardCount => usedCardList.Count;

    private void SetPrefabMap()
    {
        // 2. 프리팹 리스트를 딕셔너리로 변환
        prefabDict.Clear();
        foreach (var card in cardPrefabList)
        {
            if (!prefabDict.ContainsKey(card.Name))
            {
                prefabDict.Add(card.Name, card);
            }
        }
    }

    public void Init()
    {
        SetPrefabMap();

        // 2. 덱레시피를 보고 원본 덱(OriginCardList)을 만든다 
        DeckMaking();

        // 3. 게임 시작 상태로 리셋
        ResetDeck();
    }

    // 덱 레시피 -> originCardList 변환 함수
    private void DeckMaking()
    {
        originCardList.Clear();

        foreach (var recipe in initialDeckRecipe)
        {
            // 딕셔너리에 해당 타입의 프리팹이 있는지 확인
            for (int i = 0; i < recipe.count; i++)
            {
                originCardList.Add(recipe.name);
            }
        }

        Debug.Log($"[Deck] 초기 덱 구성 완료. 총 {originCardList.Count}장의 카드가 로드되었습니다.");
    }

    // 즉시 덱을 섞는다
    public void Shuffle()
    {
        // UsedCardList 에 카드가 없으면 섞을 필요 없음
        if (usedCardList.Count > 0)
        {
            unusedCardList.AddRange(usedCardList);
            usedCardList.Clear();
        }

        // 랜덤 섞기 
        for (int i = unusedCardList.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (unusedCardList[rand], unusedCardList[i]) = (unusedCardList[i], unusedCardList[rand]);
        }
    }

    // 카드를 n장 뽑는다 > 화면에 생성 > 정렬
    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (unusedCardList.Count == 0)
            {
                Shuffle();
                if (unusedCardList.Count == 0) break;
            }

            // 1. 데이터(프리팹) 꺼내기
            CardName cardName = unusedCardList[^1];
            unusedCardList.RemoveAt(unusedCardList.Count - 1);
            // 2. 화면에 생성 (Instantiate)
            Card newCard = Instantiate(prefabDict[cardName], handParent);
            // 3. 핸드 리스트에 등록 (화면 관리용)
            currentHandList.Add(newCard);
        }

        // 4. 다 뽑았으면 정렬
        AlignHand();
    }

    //핸드 카드 정렬
    private void AlignHand()
    {
        int count = currentHandList.Count;
        if (count == 0) return;

        // 중앙 정렬 공식: -(전체너비 / 2) + (순서 * 간격)
        float totalWidth = (count - 1) * cardSpacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < count; i++)
        {
            float xPos = startX + (i * cardSpacing);

            // Hand 오브젝트(부모) 기준 로컬 좌표로 배치
            currentHandList[i].transform.localPosition = new Vector3(xPos, 0, 0);

            // 겹칠 때 순서 정리 (오른쪽이 위로 오게)
            // currentHandList[i].GetComponent<SortingGroup>().sortingOrder = i; 
        }
    }
    // 사용한 카드 UsedCardList 로 보내기
    public void Discard(Card usedCard)
    {
        // 1. 데이터 처리: 원본 프리팹을 찾아 UsedCardList에 넣기
        usedCardList.Add(usedCard.Name);

        // 2. 핸드에서 제거
        Debug.Assert(currentHandList.Contains(usedCard));
        currentHandList.Remove(usedCard);

        // 3. 사용한 카드 카드 오브젝트 제거
        Destroy(usedCard.gameObject);

        // 4. 다시 정렬
        AlignHand();
    }

    //턴끝나면 패의 카드 모두 버림
    public void DiscardHand()
    {
        for (int i = currentHandList.Count - 1; i >= 0; i--)
        {
            Discard(currentHandList[i]);
        }
    }

    // 원본 리스트를 기반으로 게임 덱 초기화
    public void ResetDeck()
    {
        unusedCardList.Clear();
        usedCardList.Clear();

        // Origin(프리팹 원본들)을 Unused로 복사
        foreach (var cardName in originCardList)
            unusedCardList.Add(cardName);

        // 섞기
        Shuffle();
    }

    // 카드 추가
    public void AddCard(CardName cardName)
    {
        originCardList.Add(cardName);
    }

    // 카드 제거
    public void RemoveCard(CardName cardName)
    {
        if (originCardList.Contains(cardName))
            originCardList.Remove(cardName);
    }


}