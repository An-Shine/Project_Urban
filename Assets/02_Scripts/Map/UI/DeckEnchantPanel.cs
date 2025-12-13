using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeckEnchantPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panelObject;      // 패널 전체 (켜고 끄기용)
    [SerializeField] private Transform contentArea;       // ScrollView의 Content (카드가 생성될 위치)
    [SerializeField] private GameObject cardSlotPrefab;   // 카드 슬롯 프리팹 (CardSlotUI 포함)

    // 현재 플레이어의 덱 데이터를 참조하기 위한 변수
    private List<CardData> myDeck; 

    private void Start()
    {
        // 시작 시 패널은 꺼둠
        panelObject.SetActive(false);
    }

    // 외부에서 이 함수를 호출하여 강화팝업 오픈
    public void OpenEnchantPanel(List<CardData> playerDeck)
    {
        myDeck = playerDeck;
        panelObject.SetActive(true);
        
        RenderDeck();
    }

    // 패널 닫기
    public void CloseEnchantPanel()
    {
        panelObject.SetActive(false);
    }

    // 덱 표시
    private void RenderDeck()
    {
        // 1. 초기화
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }

        // 2. 덱에 있는 카드만큼 슬롯 생성
        foreach (CardData card in myDeck)
        {
            GameObject slotObj = Instantiate(cardSlotPrefab, contentArea);
            
            // CardSlotUI 스크립트를 가져와서 데이터 세팅
            //CardSlotUI slotUI = slotObj.GetComponent<CardSlotUI>();
            //if (slotUI != null)
            {
               // slotUI.SetCard(card); // 카드 이미지, 텍스트 등 설정
            }

            // 3. 버튼 기능 연결
            Button btn = slotObj.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => OnCardSelectedToEnchant(card));
            }
        }
    }

    // 카드가 선택되었을 때 실행되는 강화 로직
    private void OnCardSelectedToEnchant(CardData selectedCard)
    {
       // Debug.Log($"[강화] {selectedCard.cardName} 카드를 선택했습니다.");

        // --- 여기에 실제 강화 로직을 구현합니다 ---
        // 예: selectedCard.attackPower += 3;
        // 예: ResourceManager.Instance.SpendGold(100);
        
        // 강화를 마친 후 UI를 갱신하거나 패널을 닫습니다.
        // RenderDeck(); // 변경된 수치를 반영하기 위해 다시 그림
        // CloseEnchantPanel(); // 혹은 강화를 마치고 창 닫기
    }
}