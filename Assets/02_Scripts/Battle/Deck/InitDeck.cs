using System.Collections.Generic;
using UnityEngine;

public class InitDeck : MonoBehaviour
{    
    [SerializeField] private Deck deck; 
    
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
        public Card prefab; 
    }
   
    public List<DeckRecipe> initialDeckRecipe = new List<DeckRecipe>();
    
    public List<CardPrefabMapping> cardPrefabs = new List<CardPrefabMapping>();

    //Dictionary 형태로 저장
    
    private Dictionary<CardType, Card> prefabDict = new Dictionary<CardType, Card>();

    private void Awake()
    {
        // 1. 프리팹 리스트를 딕셔너리로 변환
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
        // 2. 게임 시작 시 덱 정보를 생성해서 Deck에게 전달
        PassDeckInfoToDeck();
    }
    
    private void PassDeckInfoToDeck()
    {
        List<Card> deckInfoList = new List<Card>();

        // 덱레시피 확인
        foreach (var recipe in initialDeckRecipe)
        {
            // 해당 타입의 프리팹이 있는지 확인
            if (prefabDict.ContainsKey(recipe.type))
            {
                Card prefab = prefabDict[recipe.type];

                // 설정된 수량만큼 프리팹 원본 리스트에 추가
                for (int i = 0; i < recipe.count; i++)
                {                    
                    deckInfoList.Add(prefab); 
                }
            }            
        }

        // 완성된 프리팹 리스트를 Deck에게 전달
        if (deck != null)
        {
            deck.SetOriginalDeck(deckInfoList);
        }
    }
}