using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events; // 클릭 이벤트 전달을 위해 필수

public class CardEnchantSlot : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image cardImage;   // 카드 이미지 표시용
    [SerializeField] private Button slotButton; // 클릭 기능을 담당할 버튼

    // 이 슬롯이 가지고 있는 카드 데이터
    private CardDataEntry _cardData;

    // [중요] 이 함수가 없어서 에러가 났던 것입니다!
    // DeckEnchantPanel에서 이 함수를 호출해서 카드 정보와 클릭 기능을 넣어줍니다.
    public void SetEnchantItem(CardDataEntry data, UnityAction<CardDataEntry> onClickCallback)
    {
        if (data == null) return;

        _cardData = data;

        // 1. 이미지 설정
        if (cardImage != null)
        {
            cardImage.sprite = data.cardSprite;
            cardImage.color = Color.white; 
        }

        // 2. 버튼 컴포넌트 확인 및 추가
        if (slotButton == null)
        {
            slotButton = GetComponent<Button>();
            if (slotButton == null) slotButton = gameObject.AddComponent<Button>();
        }

        // 3. 버튼 클릭 이벤트 연결
        slotButton.onClick.RemoveAllListeners(); // 기존 연결 제거 (중복 방지)

        // 버튼을 클릭하면 -> 패널이 시킨 일(팝업 열기)을 수행해라!
        slotButton.onClick.AddListener(() => onClickCallback(_cardData));
    }
}