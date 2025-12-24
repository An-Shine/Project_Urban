using Michsky.UI.Dark;
using UnityEngine;

[RequireComponent(typeof(ModalWindowManager))]
public class EnchantConfirmPopup : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] private UICard BeforeCardUI; // 왼쪽: 선택한 카드
    [SerializeField] private UICard AfterCardUI;  // 오른쪽: 강화된 카드

    private ModalWindowManager mwManager;
    private CardDataEntry currentTarget; 

    private void Awake()
    {
         mwManager = GetComponent<ModalWindowManager>();
    }

    public void OpenPopup(CardDataEntry card)
    {         
        currentTarget = card;

        // 1. 왼쪽 카드 세팅
        BeforeCardUI.SetCardDataEntry(card); 
        AfterCardUI.SetCardDataEntry(card); 
        mwManager.ModalWindowIn();
        
    }

    public void ClosePopup()
    {
        mwManager.ModalWindowOut();
    }

    public void OnClickEnchant()
    {
        Debug.Log($"[강화 성공] {currentTarget.cardName} 강화 로직 실행!");
        ClosePopup();
    }
}