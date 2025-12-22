using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIStoreCard : MonoBehaviour, IClickable
{
    [SerializeField] private UICard uICard;
    [SerializeField] private TMP_Text priceText;

    private int price;
    private bool isSoldOut;
    private Button button;
    private CardName cardName;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetCardDataEntry(CardDataEntry cardDataEntry)
    {
        uICard.SetCardDataEntry(cardDataEntry);
        price = cardDataEntry.price;
        cardName = cardDataEntry.cardName;
        priceText.text = $"{price}C";

        GameManager.Instance.Coin.OnUpdateCoin.AddListener(UpdatePriceText);
    }

    private void UpdatePriceText(int curCoin)
    {
        if (isSoldOut) return;

        if (price > curCoin)
        {
            priceText.color = Color.red;
            button.interactable = false;
        }
    }

    public void Buy()
    {
        GameManager.Instance.UseCoin(price);
        GameManager.Instance.Deck.AddCard(cardName);

        isSoldOut = true;
        button.interactable = false;
        priceText.text = "Sold Out";
    }

    public void AddClickHandler(UnityAction handleClick)
    {
        button.onClick.AddListener(handleClick);
    }
}