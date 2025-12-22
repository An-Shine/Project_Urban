using UnityEngine;
using TMPro;
using Michsky.UI.Dark; 

public class StoreCardRemovePopup : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject popupObject; 
    [SerializeField] private TMP_Text titleText;    
    [SerializeField] private UICard targetCardUI; 

    private CardDataEntry targetCard;    
    private ModalWindowManager mwManager;
    private StoreCardRemovePanel parentPanel;

    public void OpenPopup(StoreCardRemovePanel panel, CardDataEntry card)
    {
        if (mwManager == null) mwManager = GetComponent<ModalWindowManager>();
        
        parentPanel = panel; // 패널 기억하기
        targetCard = card;

        // 1. 텍스트 변경
        titleText.text = $"{card.koreanName}을(를)\n제거하시겠습니까?";

        // 2. 카드 이미지 세팅
        if (targetCardUI != null)
        {
            targetCardUI.SetCardDataEntry(card);
        }

        // 3. 팝업 켜기
        if (mwManager != null) mwManager.ModalWindowIn();
        else gameObject.SetActive(true);
    }

    public void OnClickRemove()
    {  
        if (targetCard != null)
        {
            // 실제 덱 데이터에서 제거 (ProtoTypeDeck 연결)
            ProtoTypeDeck.Instance.RemoveCard(targetCard);
            
            // 뒤에 있는 패널 새로고침 (카드 제거 반영)           
            parentPanel.RenderDeck();
            
        }
       
        ClosePopup();
    }

    public void OnClickCancel()
    {
        ClosePopup();
    }

    private void ClosePopup()
    {
        if (mwManager != null) mwManager.ModalWindowOut();
        else gameObject.SetActive(false);
    }
}