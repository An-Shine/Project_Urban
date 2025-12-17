using UnityEngine;
using TMPro;
using Michsky.UI.Dark; 

public class StoreCardRemovePopup : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject popupObject; 
    [SerializeField] private TMP_Text titleText;    
  
    [SerializeField] private UICard targetCardUI; 

    private CardDataEntry _targetCard;    

    public void OpenPopup(CardDataEntry card)
    {
        _targetCard = card;

        // 1. 텍스트 변경
        titleText.text = $"{card.koreanName}을(를)\n제거하시겠습니까?";

        // 2. 카드 이미지/정보 변경 요청   
        targetCardUI.SetCardDataEntry(card);
        
        
        

        // 3. 팝업 켜기
        popupObject.SetActive(true);
        GetComponent<ModalWindowManager>().ModalWindowIn();
    }

    public void OnClickRemove()
    {  
        // 여기에 실제 삭제 로직 입력예정
       
        // 삭제 후 패널 갱신이 필요하다면 패널의 Refresh 함수 호출 필요
        
        ClosePopup();
    }

    public void OnClickCancel()
    {
        ClosePopup();
    }

    private void ClosePopup()
    {
        GetComponent<ModalWindowManager>().ModalWindowOut();
    }
}