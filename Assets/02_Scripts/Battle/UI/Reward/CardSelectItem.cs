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
        image.sprite = CardManager.Instance.GetCardSprite(cardName);
    }

    public void OnClickItem()
    {
        GameManager.Instance.Deck.AddCard(cardName);
    }
}