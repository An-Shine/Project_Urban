using UnityEngine;
using TMPro;

public class BuyCardPopup : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private TMP_Text questionText; 
    [SerializeField] private GameObject BuyCardPanel; 

    private StoreUI _storeUI;
    private int _targetSlotIndex;

    // StoreUI가 이 함수를 호출해서 팝업 오픈
    public void OpenPopup(StoreUI storeUI, int slotIndex, string cardName)
    {
        _storeUI = storeUI;
        _targetSlotIndex = slotIndex;

        // 구매문구 설정
        questionText.text = $"{cardName} buy?";

        // 팝업 켜기
        BuyCardPanel.SetActive(true);
    }

    // Yes 버튼에 연결
    public void OnClickYes()
    {
        // 상점에게 구매 확정 전달
        if (_storeUI != null)
        {
            _storeUI.ConfirmPurchase(_targetSlotIndex);
        }
        
        ClosePopup();
    }

    // No 버튼에 연결
    public void OnClickNo()
    {
        ClosePopup();
    }

    private void ClosePopup()
    {
        BuyCardPanel.SetActive(false);
    }
}