using UnityEngine;
using UnityEngine.UI; 
using System.Collections.Generic; 

public class DeckEnchantPanel : MonoBehaviour
{
    [Header("UI 연결 (필수)")]
    [SerializeField] private GameObject panelObject;    // 패널 전체 오브젝트
    [SerializeField] private Transform contentArea;     // ScrollView 안의 Content
    [SerializeField] private GameObject cardSlotPrefab; // 슬롯 프리팹 (StoreCardSlot 붙은거)
    
    [Header("팝업 연결 (필수)")]
    [SerializeField] private DeckEnchantPopup enchantPopup; // 위에서 만든 팝업 스크립트 연결

    // 상점/쉼터 버튼이 이 함수를 호출합니다.
    public void OpenEnchantPanel()
    {
        // 1. 덱 데이터 가져오기 
        List<CardDataEntry> currentDeck = ProtoTypeDeck.Instance.GetCurrentDeck();

        // 2. UI 켜고 그리기
        panelObject.SetActive(true);        
        RenderDeck(currentDeck);
    }

    public void CloseEnchantPanel()
    {
        panelObject.SetActive(false);
    }

    // 카드 목록을 버튼으로 생성하는 함수
    private void RenderDeck(List<CardDataEntry> deckToRender)
    {
        // 초기화
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }

        // 새 슬롯 만들기
        foreach (CardDataEntry entry in deckToRender)
        {
            GameObject slotObj = Instantiate(cardSlotPrefab, contentArea);

            // (1) 이미지/텍스트 설정
            StoreCardSlot slotScript = slotObj.GetComponent<StoreCardSlot>();
            if (slotScript != null)
            {
                slotScript.SetItem(entry); // 이미지 표시
            }

            // (2) 버튼 기능 추가
            Button btn = slotObj.GetComponent<Button>();
            if (btn == null)
            {
                btn = slotObj.AddComponent<Button>();
            }

            btn.onClick.AddListener(() => OnCardClicked(entry));
        }
    }

    // 버튼 클릭 시 실행
    private void OnCardClicked(CardDataEntry card)
    {
        // 1. 클릭이 되는지 확인
        Debug.Log($"🖱️ [클릭 감지됨!] 선택한 카드: {card.cardName}");

        if (enchantPopup != null)
        {
            Debug.Log("📢 팝업 열기 명령 보냄!");
            enchantPopup.OpenPopup(card); // 팝업 열기
        }
        else
        {
            Debug.LogError("❌ 오류: 인스펙터에서 Enchant Popup이 연결되지 않았습니다!");
        }
    }
}