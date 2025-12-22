using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Button을 쓰기 위해 추가

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
        List<CardDataEntry> myDeck = ProtoTypeDeck.Instance.GetCurrentDeck();

        // 3. 생성
        foreach (CardDataEntry card in myDeck)
        {
            GameObject newObj = Instantiate(uiCardPrefab, contentArea);
            newObj.transform.localScale = Vector3.one;
            
            UICard uiCard = newObj.GetComponent<UICard>();

            // 데이터 세팅
            uiCard.SetCardDataEntry(card);

            // 버튼 직접 찾아서 연결
            Button btn = newObj.GetComponentInChildren<Button>();
                       
            
            btn.interactable = true; // 버튼 켜기
            btn.onClick.RemoveAllListeners();
            // 클릭 시 OnCardClicked 실행
            btn.onClick.AddListener(() => OnCardClicked(card));
            
        }
    }

    private void OnCardClicked(CardDataEntry card)
    {
        removePopup.OpenPopup(this, card);
    }
}