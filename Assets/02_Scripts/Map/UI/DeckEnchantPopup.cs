using UnityEngine;
using UnityEngine.UI; // 이게 없으면 Image, Button에서 에러남

public class DeckEnchantPopup : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject popupObject; 
    [SerializeField] private Image targetCardImage;  

    private CardDataEntry currentTarget; // 현재 강화 대기 중인 카드

    public void OpenPopup(CardDataEntry card)
    {
        currentTarget = card;

        // 카드 이미지 세팅
        if (targetCardImage != null && card != null)
        {
            targetCardImage.sprite = card.cardSprite;
        }

        popupObject.SetActive(true);
    }

    // 아니오 버튼
    public void ClosePopup()
    {
        popupObject.SetActive(false);
    }

    // "예(강화)" 버튼
    public void OnClickEnchant()
    {
        if (currentTarget != null)
        {
            Debug.Log($"✨ [강화 성공] {currentTarget.cardName} 강화 완료!");
            // 여기에 실제 강화 로직 추가 (예: 공격력 증가 등)
        }
        
        ClosePopup();
    }
}