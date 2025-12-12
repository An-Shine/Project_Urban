using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CardSelectItem : MonoBehaviour
{
    private CardName cardName;

    public void SetCardInfo(CardName cardName)
    {
        this.cardName = cardName;

        Image image = GetComponent<Image>();
        Sprite sprite = CardManager.Instance.GetCardSprite(cardName);
        image.sprite = sprite;
    }

    public void OnClickItem()
    {
        GameManager.Instance.Deck.AddCard(cardName);
    }
}