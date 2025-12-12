using System.Collections.Generic;
using UnityEngine;

public class DeckCheckUI : MonoBehaviour
{
    [SerializeField] private Transform contentParent;     
    [SerializeField] private DeckCheckImage cardItemPrefab;
    [SerializeField] private GameObject DeckCheckPanel;

    private void OnEnable()
    {
        // 패널이 켜질 때마다 덱 정보를 갱신합니다.
        RefreshDeck();
    }

    public void RefreshDeck()
    {
        // 1. 안전 장치
        if (GameManager.Instance == null || GameManager.Instance.Deck == null) 
        {
            Debug.LogWarning("덱연결안됨");
            return;
        }

        // 2. 초기화
        int childCount = contentParent.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = contentParent.GetChild(i);           
            Destroy(child.gameObject); 
        }

        // 3. 현재 덱의 모든 카드 가져오기
        List<CardName> myDeck = GameManager.Instance.Deck.GetAllCards();

        // 4. 카드 생성해서 Content 안에 넣기
        foreach (CardName cardName in myDeck)
        {
            DeckCheckImage item = Instantiate(cardItemPrefab, contentParent);
            item.Setup(cardName);
        }
    }

    // 닫기 버튼에 연결할 함수
    public void CloseDeckCheckPanel()
    {
        DeckCheckPanel.SetActive(false);
    }

    public void OpenDeckCheckPanel()
    {
        DeckCheckPanel.SetActive(true);
    }
}