using System.Collections.Generic;
using UnityEngine;

public class StoreCardRemovePanel : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject panelObject; 
    [SerializeField] private Transform contentArea; 
    [SerializeField] private GameObject uiCardPrefab; 
    [SerializeField] private StoreCardRemovePopup removePopup; 

    public void OpenRemovePanel()
    {
        panelObject.SetActive(true);
        RenderDeck();
    }

    public void CloseRemovePanel()
    {
        panelObject.SetActive(false);
    }

    public void RenderDeck()
    {
        // 1. 기존 슬롯 삭제
        foreach (Transform child in contentArea) Destroy(child.gameObject);

        // 2. 덱 가져오기
        if (ProtoTypeDeck.Instance == null) return;
        List<CardDataEntry> myDeck = ProtoTypeDeck.Instance.GetCurrentDeck();

        // 3. 생성
        foreach (CardDataEntry card in myDeck)
        {
            GameObject newObj = Instantiate(uiCardPrefab, contentArea);
            newObj.transform.localScale = Vector3.one;
            UICard uiCard = newObj.GetComponent<UICard>();

            if (uiCard != null)
            {
                CardDataEntry currentCard = card; 
                
                uiCard.SetCardDataEntry(currentCard, (data) => OnCardClicked(data));
            }
        }
    }

    private void OnCardClicked(CardDataEntry card)
    {    
        // 팝업 열기
        removePopup.OpenPopup(card);
    }
}