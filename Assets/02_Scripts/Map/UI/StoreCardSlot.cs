using UnityEngine;
using UnityEngine.UI;

public class StoreCardSlot : MonoBehaviour
{
    [Header("Resource")]
    [SerializeField] private Image cardImage;          // 카드 이미지
    [SerializeField] private GameObject priceButtonPrefab; // 가격 버튼
    
    private GameObject _currentButtonObj;
    private Button _currentButtonComp;
    private Text _currentPriceText;

    private CardName _cardName;
    private int _price;
    private bool _isSoldOut = false;

    // StoreUI에서 호출
    public void SetStoreItem(CardName cardName, int price)
    {
        _cardName = cardName;
        _price = price;
        _isSoldOut = false;

        // 1. 초기화
        if (_currentButtonObj != null)
        {
            Destroy(_currentButtonObj);
        }

        // 2. 카드 이미지 설정 & 색상 초기화
        cardImage.sprite = CardManager.Instance.GetCardSprite(cardName);
        cardImage.color = Color.white; 
        cardImage.gameObject.SetActive(true);

        // 3. 버튼 생성
        _currentButtonObj = Instantiate(priceButtonPrefab, transform);
        
        // 생성된 버튼이 카드 이미지 밑으로 오도록 순서 정렬 
        _currentButtonObj.transform.SetAsLastSibling(); 

        // 4. 생성된 버튼에서 컴포넌트 가져오기 
        _currentButtonComp = _currentButtonObj.GetComponent<Button>();
        _currentPriceText = _currentButtonObj.GetComponentInChildren<Text>();

        // 5. 버튼 기능 및 텍스트 설정
        _currentPriceText.text = $"{_price} G";
        _currentPriceText.color = Color.black; 
        
        // 버튼 클릭 이벤트 연결
        _currentButtonComp.onClick.AddListener(OnClickBuy);
        _currentButtonComp.interactable = true;
    }

    // 버튼 클릭 시 실행
    public void OnClickBuy()
    {
        if (_isSoldOut) return;

        if (GameManager.Instance.Coin >= _price)
        {
            BuyCard();
        }
        else
        {
            Debug.Log($"돈 부족! (보유: {GameManager.Instance.Coin}, 필요: {_price})");
        }
    }

    private void BuyCard()
    {
        // 1. 결제 및 카드 지급
        GameManager.Instance.AddCoin(-_price);
        GameManager.Instance.Deck.AddCard(_cardName);

        // 2. 품절 처리
        _isSoldOut = true;

        // 3. UI 변경
        _currentPriceText.text = "Sold Out";
        _currentPriceText.color = Color.red;

        _currentButtonComp.interactable = false; // 버튼 비활성화 (회색 처리)

        cardImage.color = Color.gray; // 이미지 회색 처리

        Debug.Log($"{_cardName} 구매 완료!");
    }
}