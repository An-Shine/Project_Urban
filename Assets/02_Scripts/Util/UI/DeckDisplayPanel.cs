using UnityEngine;

using System.Collections.Generic;
using System.Linq;

public class DeckDisplayPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform displayArea;       // ScrollView의 Content
    [SerializeField] private GameObject cardPrefab;   // 카드 슬롯 프리팹

    private int prevCardCount;

   public void OpenDeckDisplay()
    {
        // 1. GameManager 체크
        if (GameManager.Instance == null)
        {
            Debug.LogError("DeckDisplayPanel: GameManager.Instance가 없습니다!");
            return;
        }

        // 2. 덱 체크 
        if (GameManager.Instance.Deck == null)
        {
            Debug.LogError("DeckDisplayPanel: GameManager.Instance.Deck이 초기화되지 않았습니다!");
            return;
        }

        IEnumerable<CardName> receivedDeck = GameManager.Instance.Deck.CardList;
        
        // 3. 카드 리스트 자체 체크
        if (receivedDeck == null)
        {
            Debug.LogError("DeckDisplayPanel: 카드 리스트(CardList)가 Null입니다!");
            return;
        }

        int curCardCount = receivedDeck.Count();

        if (prevCardCount != curCardCount)
        {
            RenderDeck(receivedDeck);
            prevCardCount = curCardCount;
        }

        gameObject.SetActive(true);
    }

    public void CloseDeckDisplay()
    {
        gameObject.SetActive(false);
    }

    private void RenderDeck(IEnumerable<CardName> deckToRender)
    {
        // 1. 초기화
        foreach (Transform child in displayArea)
        {
            Destroy(child.gameObject);
        }

        // 2. 슬롯 생성
        foreach (CardName cardName in deckToRender)
        {
            GameObject slotObj = Instantiate(cardPrefab, displayArea);
            slotObj.transform.localScale = Vector3.one;

            // 프리팹에 StoreCardSlot 컴포넌트가 있는지 확인
            //StoreCardSlot slotScript = slotObj.GetComponent<StoreCardSlot>();
            UICard cardScript = slotObj.GetComponent<UICard>();

            cardScript.SetCardDataEntry(CardManager.Instance.GetCardData(cardName));
        }
    }
}