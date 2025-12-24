using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StoreCardUI : MonoBehaviour, IClickable
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
        UpdatePriceText(GameManager.Instance.Coin.CurrentCoin);
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
        // 카드 비활성화 및 품절 처리
        isSoldOut = true;
        button.interactable = false;
        priceText.text = "Sold Out";        

        // 코인 사용 처리 및 덱에 카드 추가
        GameManager.Instance.UseCoin(price);
        GameManager.Instance.Deck.AddCard(cardName);
    }

    public void AddClickHandler(UnityAction handleClick)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(handleClick);
    }
}