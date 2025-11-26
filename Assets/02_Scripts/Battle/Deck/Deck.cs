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

    //핸드 설정
    [SerializeField] private Transform handParent; // 카드가 생성되는 핸드 오브젝트
    [SerializeField] private float cardSpacing = 2.5f; // 카드 간격

    // 덱 딕셔너리
    private Dictionary<CardType, Card> prefabDict = new Dictionary<CardType, Card>();
    
    
    // 게임 데이터 
    // 원본 덱 
    [SerializeField] private List<Card> originCardList = new List<Card>();

    // 뽑을 덱 
    [SerializeField] private List<Card> unusedCardList = new List<Card>();

    // 사용한 카드리스트
    [SerializeField] private List<Card> usedCardList = new List<Card>();

    //현재 핸드에 있는 카드리스트
    private List<Card> currentHandList = new List<Card>();

   
    public int UnusedCardCount => unusedCardList.Count;
    public int UsedCardCount => usedCardList.Count;

  

    private void Awake()
    {
        //Hand 오브젝트 위치에 카드 프리펩 생성
        // 1. Hand 오브젝트 찾기
        if (handParent == null)
        {
            GameObject handObj = GameObject.Find("Hand"); // 이름으로 찾기
            if (handObj != null)
            {
                handParent = handObj.transform;
            }
            else
            {
                // 없으면 새로 만들기
                handObj = new GameObject("Hand");
                handObj.transform.position = new Vector3(0, -4, 0); // 화면 하단
                handParent = handObj.transform;
            }
        }

        // 2. 프리팹 리스트를 딕셔너리로 변환
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
            if (prefabDict.ContainsKey(recipe.type))
            {
                Card prefab = prefabDict[recipe.type];

                // 설정된 수 만큼 프리팹을 원본 리스트에 등록
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
            Card temp = unusedCardList[i];
            unusedCardList[i] = unusedCardList[rand];
            unusedCardList[rand] = temp;        
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
            int lastIndex = unusedCardList.Count - 1;
            Card cardPrefab = unusedCardList[lastIndex];
            unusedCardList.RemoveAt(lastIndex);

            // 2. 화면에 생성 (Instantiate)
            Card newCard = Instantiate(cardPrefab, handParent);
            
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
            
            // (옵션) 겹칠 때 순서 정리 (오른쪽이 위로 오게)
            // currentHandList[i].GetComponent<SortingGroup>().sortingOrder = i; 
        }
    }
    // 사용한 카드 UsedCardList 로 보내기
    public void Discard(Card usedCardInstance)
    {        
        // 1. 데이터 처리: 원본 프리팹을 찾아 UsedCardList에 넣기
        CardType type = usedCardInstance.Type; 

        if (prefabDict.ContainsKey(type))
        {
            Card originalPrefab = prefabDict[type];
            usedCardList.Add(originalPrefab);
        }
        else
        {
            Debug.LogError("프리팹을 찾을 수 없습니다.");
        }

        // 2. 핸드에서 제거
        if (currentHandList.Contains(usedCardInstance))
        {
            currentHandList.Remove(usedCardInstance);
        }

        // 3. 사용한 카드 프리펩 제거
        Destroy(usedCardInstance.gameObject);

        // 4. 다시 정렬
        AlignHand();
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

    
}