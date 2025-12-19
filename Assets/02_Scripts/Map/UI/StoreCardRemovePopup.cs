using UnityEngine;
using TMPro;
using Michsky.UI.Dark; 

public class StoreCardRemovePopup : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject popupObject; 
    [SerializeField] private TMP_Text titleText;    
    [SerializeField] private UICard targetCardUI; // 팝업 화면 카드 프리팹 연결

    private CardDataEntry _targetCard;    
    private ModalWindowManager mwManager;

    public void OpenPopup(CardDataEntry card)
    {
        mwManager = GetComponent<ModalWindowManager>();
        
        _targetCard = card;

        // 1. 텍스트 변경
        
        titleText.text = $"{card.koreanName}을(를)\n제거하시겠습니까?";

        // 2. 카드 이미지/정보 변경        
        
        targetCardUI.SetCardDataEntry(card);
        
        // 3. 팝업 켜기
        mwManager.ModalWindowIn();        
    }

    public void OnClickRemove()
    {  
        if (_targetCard != null)
        {
            Debug.Log($"[카드 제거] {_targetCard.koreanName} 제거 완료!");
            // 실제 덱에서 카드를 제거하는 로직 추가 예정
            // GameManager.Instance.Deck.RemoveCard(_targetCard);
            
            // 제거 후 패널 갱신이 필요하다면 여기서 호출
            // transform.parent.GetComponent<StoreCardRemovePanel>().RenderDeck();
        }
       
        ClosePopup();
    }

    public void OnClickCancel()
    {
        ClosePopup();
    }

    private void ClosePopup()
    {
        mwManager.ModalWindowOut();        
    }
}