using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class UICard : MonoBehaviour
{
    [SerializeField] private TMP_Text cardTitle;
    [SerializeField] private TMP_Text cardDesc;
    [SerializeField] private Image cardImage;
    [SerializeField] private Button cardButton;

    [Header("상점UI 옵션")]
    [SerializeField] private TMP_Text priceText; // 가격 텍스트

    private CardDataEntry currentData;
    
    public void SetCardDataEntry(CardDataEntry data, UnityAction<CardDataEntry> onClickCallback = null)
    {
        currentData = data;
        
        cardImage.sprite = data.cardSprite;
        cardTitle.text = data.koreanName;
        cardDesc.text = data.description;

        // 상점용 UI 숨기기
        priceText.gameObject.SetActive(false);        

        // 버튼 이벤트 연결
        cardButton.interactable = true;
        cardButton.onClick.RemoveAllListeners();
        
        // 콜백이 있으면 연결, 없으면 버튼 끄기
        if (onClickCallback != null)
        {
            cardButton.onClick.AddListener(() => onClickCallback(currentData));
        }
        else
        {
            cardButton.interactable = false;
        }
    }

    // 상점UI 기능
    public void SetStoreMode(int price)
    {
        // 가격 텍스트 켜고 금액 표시
        priceText.gameObject.SetActive(true);
        priceText.text = $"{price} G";        
    }

    // 품절 처리
    public void SetSoldOut()
    {
        cardButton.interactable = false;
        priceText.text = "Sold Out";        
    }
}