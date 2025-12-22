using Michsky.UI.Dark;
using UnityEngine;
using UnityEngine.UI; 

public class DeckEnchantPopup : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UICard BeforeCardUI; // 왼쪽: 선택한 카드
    [SerializeField] private UICard AfterCardUI;  // 오른쪽: 강화된 카드
    [SerializeField] private GameObject popupObject; 

    private ModalWindowManager mwManager;
    private CardDataEntry currentTarget; 
   
    private void Awake()
    {
    }

    public void OpenPopup(CardDataEntry card)
    {         
        mwManager = GetComponent<ModalWindowManager>();
        

        currentTarget = card;

        // 1. 왼쪽 카드 세팅
        BeforeCardUI.SetCardDataEntry(card);

        // 2. 오른쪽 카드 세팅      
        AfterCardUI.SetCardDataEntry(card); 
        
         mwManager.ModalWindowIn();
        
    }

    public void ClosePopup()
    {
        if (mwManager != null) mwManager.ModalWindowOut();
        else gameObject.SetActive(false);
    }

    public void OnClickEnchant()
    {
        Debug.Log($"[강화 성공] {currentTarget.cardName} 강화 로직 실행!");
        ClosePopup();
    }
}