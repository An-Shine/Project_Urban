using UnityEngine;
using TMPro;
using Michsky.UI.Dark;

public class BuyCardPopup : MonoBehaviour
{    
    [SerializeField] private TMP_Text questionText; 
    [SerializeField] private GameObject buyCardPanel; 
    [SerializeField] private UICard targetCardUI;

    private StoreUI storeUI;

    public void OpenPopup(StoreUI storeUIInstance, CardDataEntry cardData)
    {
        storeUI = storeUIInstance;

        questionText.text = $"{cardData.koreanName} 구매하시겠습니까?";
        targetCardUI.SetCardDataEntry(cardData); 

        buyCardPanel.SetActive(true);
        GetComponent<ModalWindowManager>().ModalWindowIn();
    }

    public void OnClickYes()
    {
        storeUI.ConfirmPurchase();
        ClosePopup();
    }

    public void OnClickNo()
    {
        ClosePopup();
    }

    private void ClosePopup()
    {
        GetComponent<ModalWindowManager>().ModalWindowOut();       
    }   
}