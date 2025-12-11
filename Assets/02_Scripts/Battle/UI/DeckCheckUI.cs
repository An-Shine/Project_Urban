using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq; // 정렬(Sorting)을 위해 필요

public class DeckCheckUI : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject popupPanel;   // 
    [SerializeField] private Transform contentArea;   // Scroll View 안의 Content 오브젝트
    [SerializeField] private Button closeButton;      // 닫기 버튼

    [Header("프리팹 설정")]
    [SerializeField] private GameObject emptySlotPrefab; // 카드를 담을 빈 UI 슬롯 

    private void Start()
    {
        // 시작 시 팝업 끄기 및 버튼 연결
        popupPanel.SetActive(false);
        
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseDeckView);
    }

    // 팝업 열기
    public void OpenDeckView()
    {
        popupPanel.SetActive(true);
        RefreshDeck();
    }

    // 팝업 닫기
    public void CloseDeckView()
    {
        popupPanel.SetActive(false);
    }

    private void RefreshDeck()
    {
        // 1. 초기화
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }

        // 2. GameManager에서 현재 덱 정보 가져오기
        if (GameManager.Instance == null) return;
        List<CardName> myDeck = GameManager.Instance.GetGlobalDeck();

        // 3. 정렬 
        List<CardName> sortedDeck = myDeck.OrderBy(x => x.ToString()).ToList();

        // 4. 카드 생성 루프
        foreach (CardName cardName in sortedDeck)
        {
            // 4-1. 팩토리에서 카드 원본 프리팹 가져오기
            Card cardPrefab = CardFactory.Instance.GetCardPrefab(cardName);

            if (cardPrefab != null)
            {
                // 4-2. UI 정렬을 위한 '빈 슬롯' 먼저 생성
                GameObject slotObj = Instantiate(emptySlotPrefab, contentArea);
                
                // 4-3. 실제 3D 카드를 슬롯의 자식으로 생성
                GameObject cardVisual = Instantiate(cardPrefab.gameObject, slotObj.transform);

                cardVisual.transform.localPosition = new Vector3(0, 0, -500f); 
                cardVisual.transform.localScale = Vector3.one * 120f; 
                cardVisual.transform.localRotation = Quaternion.identity;
                
                ChangeLayersRecursively(cardVisual, "UI");
            }
        }
    }

    private void ChangeLayersRecursively(GameObject target, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer == -1) return;
        target.layer = layer;
        foreach (Transform child in target.transform) 
            ChangeLayersRecursively(child.gameObject, layerName);
    }
}