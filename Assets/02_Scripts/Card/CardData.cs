using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[CreateAssetMenu(fileName = "CardData", menuName = "Card/CardData", order = 0)]
public class CardData : ScriptableObject
{
    [SerializeField] private string spriteBasePath = "03_Images/Cards";
    [SerializeField] private string prefabBasePath = "05_Prefabs/Cards";
    [SerializeField] private bool useElementFolder = true;
    [SerializeField] private List<CardDataEntry> cards = new();

    // Element별로 묶고, CardName을 키로 하는 Dictionary
    private Dictionary<Element, Dictionary<CardName, CardDataEntry>> cardDataMap;

    private void OnEnable()
    {
        BuildCardDataMap();
    }

    private void BuildCardDataMap()
    {
        cardDataMap = new Dictionary<Element, Dictionary<CardName, CardDataEntry>>();

        foreach (CardDataEntry entry in cards)
        {
            if (!cardDataMap.ContainsKey(entry.element))
            {
                cardDataMap[entry.element] = new Dictionary<CardName, CardDataEntry>();
            }

            cardDataMap[entry.element][entry.cardName] = entry;
        }
    }

    public Sprite GetCardSprite(CardName cardName)
    {
        CardDataEntry entry = GetCardData(cardName);
        return entry?.cardSprite;
    }

    public Card GetCardPrefab(CardName cardName)
    {
        CardDataEntry entry = GetCardData(cardName);
        return entry?.cardPrefab;
    }

    public CardDataEntry GetCardData(CardName cardName)
    {
        if (cardDataMap == null)
            BuildCardDataMap();

        foreach (var elementDict in cardDataMap.Values)
        {
            if (elementDict.ContainsKey(cardName))
                return elementDict[cardName];
        }

        throw new KeyNotFoundException($"CardData not found for CardName: {cardName}");
    }

    public List<CardName> GetAllCardNames()
    {
        // 전체카드 이름만 다 뽑아서 리스트로 만들어서 StoreUI에 전달
        return cards.Select(entry => entry.cardName).ToList();
    }

    public Dictionary<CardName, CardDataEntry> GetCardsByElement(Element element)
    {
        if (cardDataMap == null)
            BuildCardDataMap();

        return cardDataMap[element]; // 없으면 자동으로 KeyNotFoundException 발생
    }

    public IEnumerable<CardDataEntry> GetAllCards()
    {
        return cards;
    }

#if UNITY_EDITOR
    [ContextMenu("Import From JSON")]
    public void ImportFromJson()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select Card Data JSON", Application.dataPath, "json");
        
        if (string.IsNullOrEmpty(jsonPath))
            return;

        try
        {
            string jsonText = File.ReadAllText(jsonPath);
            JsonWrapper wrapper = JsonUtility.FromJson<JsonWrapper>(jsonText);
            
            cards.Clear();

            foreach (JsonCardData jsonCard in wrapper.cards)
            {
                // CardName 파싱
                if (!Enum.TryParse(jsonCard.cardName, true, out CardName parsedCardName))
                {
                    Debug.LogWarning($"Invalid CardName: {jsonCard.cardName}");
                    continue;
                }

                // Element 파싱
                if (!Enum.TryParse(jsonCard.element, true, out Element parsedElement))
                {
                    Debug.LogWarning($"Invalid Element: {jsonCard.element}");
                    continue;
                }

                CardDataEntry entry = new CardDataEntry
                {
                    cardName = parsedCardName,
                    element = parsedElement,
                    isSpecial = jsonCard.isSpecial,
                    description = jsonCard.description ?? string.Empty
                };

                // Sprite 로드
                string spritePath = GetAssetPath(spriteBasePath, jsonCard.cardName, parsedElement, ".png");
                entry.cardSprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                if (entry.cardSprite == null)
                    Debug.LogWarning($"Sprite not found: {spritePath}");

                // Prefab 로드
                string prefabPath = GetAssetPath(prefabBasePath, jsonCard.cardName, parsedElement, ".prefab");
                entry.cardPrefab = AssetDatabase.LoadAssetAtPath<Card>(prefabPath);
                if (entry.cardPrefab == null)
                    Debug.LogWarning($"Prefab not found: {prefabPath}");

                cards.Add(entry);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            // Dictionary 재생성
            BuildCardDataMap();

            Debug.Log($"Successfully imported {cards.Count} cards from JSON");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to import JSON: {e.Message}");
        }
    }

    private string GetAssetPath(string basePath, string fileName, Element element, string extension)
    {
        string relativePath = useElementFolder
            ? Path.Combine(basePath, element.ToString(), fileName + extension)
            : Path.Combine(basePath, fileName + extension);
        
        string fullPath = Path.Combine("Assets", relativePath);
        Debug.Log($"Looking for asset: {fullPath}");
        return fullPath;
    }
#endif
}

[Serializable]
public class CardDataEntry
{
    public CardName cardName;
    public Sprite cardSprite;
    public Element element;
    public bool isSpecial;
    public Card cardPrefab;
    
    [TextArea] public string description;
}

#if UNITY_EDITOR
[Serializable]
public class JsonWrapper
{
    public List<JsonCardData> cards;
}

[Serializable]
public class JsonCardData
{
    public string cardName;
    public string element;
    public bool isSpecial;
    public string description;    
}
#endif
