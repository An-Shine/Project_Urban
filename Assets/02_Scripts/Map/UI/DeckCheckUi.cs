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

    private void Start()
    {
        panelObject.SetActive(false);
    }

    public void OpenDeckUI()
    {
        Debug.Log("[DeckCheckUI] UI 열기 시도...");

        // 1. ProtoTypeDeck에서 데이터 가져오기
        if (ProtoTypeDeck.Instance == null)
        {
            Debug.LogError("[DeckCheckUI] ProtoTypeDeck 인스턴스를 찾을 수 없습니다! 씬에 ProtoTypeDeck이 있나요?");
            return;
        }

        List<CardDataEntry> receivedDeck = ProtoTypeDeck.Instance.GetCurrentDeck();

        // [디버깅] 몇 장을 받아왔는지 콘솔에 출력
        Debug.Log($"[DeckCheckUI] ProtoTypeDeck에서 받아온 카드 개수: {receivedDeck.Count}장");

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
        // 1. 기존 슬롯 삭제 (초기화)
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
            
            // 프리팹에 StoreCardSlot 컴포넌트가 있는지 확인
            StoreCardSlot slotScript = slotObj.GetComponent<StoreCardSlot>();

            if (slotScript != null)
            {
                slotScript.SetItem(entry);
            }
            else
            {
                Debug.LogError("[DeckCheckUI] 프리팹에 'StoreCardSlot' 스크립트가 붙어있지 않습니다!");
            }
        }
    }
}