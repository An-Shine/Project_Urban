using UnityEngine;
using UnityEngine.UI;

public class DeckCheckImage : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    private Sprite sprite;

    public void Setup(CardName cardName)
    {
        // 카드팩토리에서 이미지를 가져와서 보여줌
        cardImage.sprite = CardManager.Instance.GetCardSprite(cardName);

        if (sprite != null)
        {
            cardImage.sprite = sprite;
        }
        else
        {
            Debug.LogWarning($"이미지를 찾을 수 없습니다: {cardName}");
        }
    }
}