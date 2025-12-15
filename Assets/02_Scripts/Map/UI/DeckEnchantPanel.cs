using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeckEnchantPanel : MonoBehaviour
{
    [Header("UI 연결 (필수)")]
    [SerializeField] private GameObject panelObject;    // 패널 전체 오브젝트
    [SerializeField] private Transform contentArea;     // ScrollView 안의 Content
    [SerializeField] private GameObject cardSlotPrefab; // 슬롯 프리팹 (StoreCardSlot 붙은거)
    [SerializeField] private GameObject HealingPanel;


    [Header("팝업 연결 (필수)")]
    [SerializeField] private DeckEnchantPopup enchantPopup; // 위에서 만든 팝업 스크립트 연결

    // 상점/쉼터 버튼이 이 함수를 호출합니다.
    public void OpenEnchantPanel()
    {
        // 1. 덱 데이터 가져오기 
        List<CardDataEntry> currentDeck = ProtoTypeDeck.Instance.GetCurrentDeck();

        // 2. UI 켜고 그리기
        panelObject.SetActive(true);
        HealingPanel.SetActive(false);
        RenderDeck(currentDeck);
    }

    public void CloseEnchantPanel()
    {
        panelObject.SetActive(false);
    }

    // 카드 목록을 버튼으로 생성하는 함수
    private void RenderDeck(List<CardDataEntry> deckToRender)
    {
        // 1. 기존 슬롯들 삭제 (초기화)
        foreach (Transform child in contentArea) Destroy(child.gameObject);

        // 2. 새로운 슬롯 생성
        foreach (CardDataEntry entry in deckToRender)
        {
            // 프리팹 생성
            GameObject slotObj = Instantiate(cardSlotPrefab, contentArea);

            // 방금 만든 스크립트(CardEnchantSlot) 가져오기
            CardEnchantSlot slotScript = slotObj.GetComponent<CardEnchantSlot>();

            if (slotScript != null)
            {
                // [핵심] 슬롯에게 데이터와 "클릭하면 실행할 함수(OnCardClicked)"를 전달
                slotScript.SetEnchantItem(entry, OnCardClicked);
            }
            else
            {
                Debug.LogError("❌ [EnchantPanel] 프리팹에 'CardEnchantSlot' 스크립트가 없습니다!");
            }
        }
    }

    // (참고) 이 함수는 이미 DeckEnchantPanel에 작성되어 있을 겁니다.
    private void OnCardClicked(CardDataEntry card)
    {
        enchantPopup.OpenPopup(card); // 팝업 열기
        Debug.Log($"🖱️ [클릭 감지됨!] 선택한 카드: {card.cardName}");
    }
}

