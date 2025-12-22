using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using Michsky.UI.Dark;

public class BuyCardPopup : MonoBehaviour
{    
    [SerializeField] private TMP_Text questionText; 
    [SerializeField] private GameObject BuyCardPanel; 
    [SerializeField] private Image targetCardImage; 
    [SerializeField] private UICard targetCardUI;

    private StoreUI _storeUI;
    private int _targetSlotIndex;


    // cardData 전체를 받아옴
    public void OpenPopup(StoreUI storeUI, int slotIndex, CardDataEntry cardData)
    {
        _storeUI = storeUI;
        _targetSlotIndex = slotIndex;

        // 1. 텍스트 설정
        questionText.text = $"{cardData.koreanName} 구매하시겠습니까?";

        // 2. 카드 UI 설정 (UICard에게 데이터 전달)        
        targetCardUI.SetCardDataEntry(cardData); 

        // 3. 팝업 켜기
        BuyCardPanel.SetActive(true);
        GetComponent<ModalWindowManager>().ModalWindowIn();
    }

    // Yes 버튼
    public void OnClickYes()
    {
        // 상점에게 구매 확정 전달
        _storeUI.ConfirmPurchase(_targetSlotIndex);
        
        ClosePopup();
    }

    // No 버튼
    public void OnClickNo()
    {
        ClosePopup();
    }

    private void ClosePopup()
    {
        // 닫기 애니메이션 실행
        GetComponent<ModalWindowManager>().ModalWindowOut();       
    }   
}