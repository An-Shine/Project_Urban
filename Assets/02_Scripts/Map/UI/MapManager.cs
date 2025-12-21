using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement; 

public class MapManager : MonoBehaviour
{
    public enum StageType
    {
        Normal,     //기본 몬스터 스테이지
        Shelter,    //휴식 스테이지
        Store,      //상점 스테이지
        Boss        //보스 스테이지
    }

    [Header("UI 패널 연결")]
    [SerializeField] private GameObject shelterUI; // 휴식 UI
    [SerializeField] private GameObject storeUI;   // 상점 UI    
    [SerializeField] private GameObject stageSelectUI;
    [SerializeField] private GameObject CloseStoreUIButton; //상점 UI 닫기버튼

    [SerializeField] private GameObject enchantPopup;    // 카드강화 UI
    [SerializeField] private GameObject removePopup;     // 카드제거 UI
    [SerializeField] private GameObject DeckCheckPanel;  // 덱 확인 UI  
    [SerializeField] private GameObject BuyCardPopup;    // 카드구매 UI
    [SerializeField] private GameObject CardEnchantRemovePanel; //카드강화,제거용 덱확인 패널     
    [SerializeField] private GameObject EnterButton;
    [SerializeField] private GameObject ShelterPopup;    
    
    public static MapManager Instance; 

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
        bool isReturning = GameManager.Instance.IsStageSelectMode;    

        if (isReturning)
        {            
            stageSelectUI.SetActive(true);
           
            GameManager.Instance.IsStageSelectMode = false;
        }
        else
        {            
            stageSelectUI.SetActive(false);
        }        
    }

    public void InitializeCard(UICard uiCard, UnityAction onClickCallback)
    {
        // 1. 데이터 세팅
        // uiCard.SetCardDataEntry(data);

        // 2. 버튼 바로 가져오기
        Button btn = uiCard.GetComponent<Button>();

        btn.onClick.RemoveAllListeners();
        
        if (onClickCallback != null)
        {
            btn.interactable = true;
            btn.onClick.AddListener(onClickCallback);
        }
        else
        {                
            btn.interactable = false;
        }
    }

   public void ConfigureShopCard(UICard uiCard, int price)
    {
        // 1. "PriceText"라는 이름의 오브젝트(부모)를 찾습니다.
        Transform priceObj = uiCard.transform.Find("PriceText");

        if (priceObj != null)
        {
            // 부모 오브젝트를 켜줍니다.
            priceObj.gameObject.SetActive(true);
            
            // [핵심 수정] 부모한테 컴포넌트가 없으면 "자식"한테서라도 찾아옵니다!
            // (true)는 꺼져있는 자식도 찾으라는 뜻입니다.
            TMP_Text priceText = priceObj.GetComponentInChildren<TMP_Text>(true);

            if (priceText != null)
            {
                priceText.text = $"{price} G";
            }
            else
            {
                Debug.LogError($"[에러] '{uiCard.name}'의 PriceText 안에 TMP_Text 컴포넌트가 없습니다! 자식 오브젝트에 텍스트가 있는지 확인하세요.");
            }
        }
        else
        {
             // 혹시 이름을 못 찾았을 때를 대비한 로그
             Debug.LogError($"[에러] '{uiCard.name}' 안에 'PriceText'라는 이름의 오브젝트를 찾지 못했습니다.");
        }
    }

    public void SetCardSoldOut(UICard uiCard)
    {
        // 1. 버튼 찾아서 비활성화
        Button btn = uiCard.GetComponent<Button>();
        if (btn != null)
        {
            btn.interactable = false;
        }

        // 2. 텍스트 찾아서 변경
        Transform priceObj = uiCard.transform.Find("PriceText");
        
        if (priceObj != null)
        {
            priceObj.gameObject.SetActive(true);
            
            // [핵심 수정] 여기도 똑같이 자식까지 뒤져서 텍스트를 찾아냅니다.
            TMP_Text priceText = priceObj.GetComponentInChildren<TMP_Text>(true);

            if (priceText != null)
            {
                priceText.text = "Sold Out";
            }
        }
    }

    // --- 버튼 클릭 함수 ---

    public void OnClickNormal()
    {
        SceneManager.LoadScene("Battle_Scene");
    }

    public void OnClickShelter()
    {
        shelterUI.SetActive(true);
    }

    public void OnClickStore()
    {
        storeUI.SetActive(true);        
    }

    public void OnClickBoss()
    {
        SceneManager.LoadScene("Battle_Scene");
    }

    public void OpenEnchantPopup()
    {
        enchantPopup.SetActive(true);        
    }

    public void OpenRemovePopup()
    {
        removePopup.SetActive(true);
    }

    public void OpenDeckCheckPanel()
    {
        DeckCheckPanel.SetActive(true);
    }

    public void OpenBuyCardPopup()
    {
        BuyCardPopup.SetActive(true);
    }

    public void OpenCardEnchantRemovePanel()
    {
        CardEnchantRemovePanel.SetActive(true);
    }
    
    public void ClosePopup()
    {        
        transform.parent.gameObject.SetActive(false);        
    }

    public void CloseStoreUI()
    {
        storeUI.SetActive(false);
    }

    public void OpenShelterPopup()
    {
        ShelterPopup.SetActive(true);
        EnterButton.SetActive(false);
    }

    public void CloseShelterPopup()
    {
        ShelterPopup.SetActive(false);
    }
    
    public void CloseShelterPanel()
    {
        shelterUI.SetActive(false);
    }
    
    private void CloseDeckCheckPanel()
    {
        DeckCheckPanel.SetActive(false);
    }
}