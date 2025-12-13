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
        // 초기화
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // CardManager 가 가지고있는 덱리스트를 그대로받아옴        
        List<CardName> currentDeck = CardManager.Instance.GetMyDeck();

        // 가져온 리스트 생성
        foreach (CardName cardName in currentDeck)
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