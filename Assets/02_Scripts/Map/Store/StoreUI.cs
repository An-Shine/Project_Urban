using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class StoreUI : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject storePanel;
    [SerializeField] private Transform contentArea;  
    [SerializeField] private GameObject uiCardPrefab; 
    [SerializeField] private BuyCardPopup buyCardPopup; 
    [SerializeField] private TMP_Text currentCoinText;     

    [Header("Store Settings")]
    [SerializeField] private int targetCount = 6; // 상점에 진열할 개수

    // 생성된 카드 슬롯들을 보관해두는 리스트 
    private readonly List<UIStoreCard> spawnedCardList = new(); 
    
    private UICardProcess selectedCard; 
    
    private List<CardName> cachedCardList;

    private void Start()
    {        
        // 테스트용 (필요 없으면 제거)
        OpenStore();
    }
    public void OpenStore()
    {
        storePanel.SetActive(true);
        SetupStore();
    }

    public void CloseStore()
    {
        storePanel.SetActive(false);
    }

    public void SetupStore()
    {        
        // 1. 이번에 사용할 카드 리스트 준비
        cachedCardList ??= CardManager.Instance.GetAllCardNames();
        List<CardName> cardList = new(cachedCardList);

        for (int i = 0; i < targetCount; i++)
        {
            if (cardList.Count == 0) break;
            
            // 랜덤 뽑기
            int randIndex = Random.Range(0, cardList.Count);
            CardName pickedName = cardList[randIndex];
            
            (cardList[randIndex], cardList[^1]) = (cardList[^1], cardList[randIndex]);
            cardList.RemoveAt(cardList.Count - 1);

            CardDataEntry cardData = CardManager.Instance.GetCardData(pickedName);

            UIStoreCard storeCard;

            // 이미 만들어둔 슬롯이 있으면 재사용
            if (i < spawnedCardList.Count)
            {
                storeCard = spawnedCardList[i];
                storeCard.gameObject.SetActive(true); 
            }
            else
            {
                // 없으면 새로 생성 
                GameObject newObj = Instantiate(uiCardPrefab, contentArea);
                newObj.transform.localScale = Vector3.one;
                storeCard = newObj.GetComponent<UIStoreCard>();
                spawnedCardList.Add(storeCard);
            }

            storeCard.SetCardDataEntry(cardData);
            storeCard.AddClickHandler(() => 
            {
                OnClickCardSlot(storeCard, cardData);
            });
        }
    }

    public void OnClickCardSlot(UIStoreCard uIStoreCard, CardDataEntry data)
    {
        buyCardPopup.OpenPopup(uIStoreCard, data); 
    }
}