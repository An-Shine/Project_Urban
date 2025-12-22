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

    // 생성된 카드 슬롯들을 보관해두는 리스트 
    private List<UICardProcess> spawnedProcess = new List<UICardProcess>(); 
    
    private UICardProcess selectedCard; 
    
    private List<CardName> cachedAllCardList;

    private void Start()
    {
        if (CardManager.Instance != null)
        {
            cachedAllCardList = CardManager.Instance.GetAllCardNames();
        }
        
        // 테스트용 (필요 없으면 제거)
        OpenStore();
    }
    public void OpenStore()
    {
        storePanel.SetActive(true);
        SetupStore();
        UpdateCoinUI(); 
    }

    public void CloseStore()
    {
        storePanel.SetActive(false);
    }

    public void SetupStore()
    {        
        // 1. 이번에 사용할 카드 리스트 준비
        List<CardName> deckToDraw;
        if (cachedAllCardList != null) deckToDraw = new List<CardName>(cachedAllCardList);
        else deckToDraw = CardManager.Instance.GetAllCardNames(); 

        int targetCount = 6; // 상점에 진열할 개수

        for (int i = 0; i < targetCount; i++)
        {
            if (deckToDraw.Count == 0) break;
            
            // 랜덤 뽑기
            int randIndex = Random.Range(0, deckToDraw.Count);
            CardName pickedName = deckToDraw[randIndex];
            deckToDraw.RemoveAt(randIndex);

            CardDataEntry cardData = CardManager.Instance.GetCardData(pickedName);

            UICardProcess process;

            // 이미 만들어둔 슬롯이 있으면 재사용
            if (i < spawnedProcess.Count)
            {
                process = spawnedProcess[i];
                process.gameObject.SetActive(true); 
            }
            else
            {
                // 없으면 새로 생성 
                GameObject newObj = Instantiate(uiCardPrefab, contentArea);
                newObj.transform.localScale = Vector3.one;
                process = newObj.GetComponent<UICardProcess>();
                spawnedProcess.Add(process);
            }

            // 3. 데이터 초기화
            process.Initialize(cardData, CardProcessType.Shop, (data) => 
            {
                OnClickCardSlot(process, data);
            });
        }

        // 남은슬롯은 비활성화처리
        for (int i = targetCount; i < spawnedProcess.Count; i++)
        {
            spawnedProcess[i].gameObject.SetActive(false);
        }
    }

    public void OnClickCardSlot(UICardProcess cardProcess, CardDataEntry data)
    {
        if (cardProcess.CanBuy())
        {
            selectedCard = cardProcess; 
            buyCardPopup.OpenPopup(this, data); 
        }
        else
        {
            Debug.Log("코인이 부족합니다.");
        }
    }

    public void ConfirmPurchase()
    {
        if (selectedCard != null)
        {
            selectedCard.PurchaseSuccess();
            UpdateCoinUI();
            selectedCard = null;
        }
    }

    private void UpdateCoinUI()
    {
        if (GameManager.Instance != null)
             currentCoinText.text = $"Coin: {GameManager.Instance.Coin}";
    }
}