using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events; // 클릭 이벤트 전달을 위해 필수

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class CardEnchantSlot : MonoBehaviour
{
    private Image cardImage;   // 카드 이미지 표시용
    private Button slotButton; // 클릭 기능을 담당할 버튼

    // 이 슬롯이 가지고 있는 카드 데이터
    private CardDataEntry _cardData;

    private void Awake()
    {
        cardImage = GetComponent<Image>();
        slotButton = GetComponent<Button>();
    }  
    public void SetEnchantItem(CardDataEntry data, UnityAction<CardDataEntry> onClickCallback)
    {
        if (data == null) return;

        _cardData = data;

        // 1. 이미지 설정
        cardImage.sprite = data.cardSprite;
        cardImage.color = Color.white; 

        // 2. 버튼 클릭 이벤트 연결
        slotButton.onClick.RemoveAllListeners(); 
        
        slotButton.onClick.AddListener(() => onClickCallback(_cardData));
    }
}