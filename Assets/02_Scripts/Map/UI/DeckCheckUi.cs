using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeckCheckUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panelObject;      // 덱 확인 창 전체
    [SerializeField] private GameObject cardSlotUI;
    [SerializeField] private Transform contentArea;       // ScrollView의 Content
    [SerializeField] private GameObject cardSlotPrefab;   // 카드 슬롯 프리팹
   

    public void OpenDeckUI()
    {               
        List<CardDataEntry> receivedDeck = ProtoTypeDeck.Instance.GetCurrentDeck();
        
        //Debug.Log($"[DeckCheckUI] ProtoTypeDeck에서 받아온 카드 개수: {receivedDeck.Count}장");

        panelObject.SetActive(true);
        cardSlotUI.SetActive(false);
        RenderDeck(receivedDeck);
    }

    public void CloseDeckUI()
    {
        panelObject.SetActive(false);
        cardSlotUI.SetActive(true);
    }

    private void RenderDeck(List<CardDataEntry> deckToRender)
    {
        // 1. 초기화
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }

        if (deckToRender == null || deckToRender.Count == 0)
        {
            Debug.LogWarning("[DeckCheckUI] 표시할 카드가 없습니다 (리스트가 비었음).");
            return;
        }

        // 2. 슬롯 생성
        foreach (CardDataEntry entry in deckToRender)
        {
            GameObject slotObj = Instantiate(cardSlotPrefab, contentArea);
            slotObj.transform.localScale = Vector3.one;
            
            // 프리팹에 StoreCardSlot 컴포넌트가 있는지 확인
            //StoreCardSlot slotScript = slotObj.GetComponent<StoreCardSlot>();
            UICard cardScript = slotObj.GetComponent<UICard>();

            cardScript.SetCardDataEntry(entry);
        }
    }
}