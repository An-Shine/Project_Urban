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
        IEnumerable<CardName> receivedDeck = GameManager.Instance.Deck.CardList;
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