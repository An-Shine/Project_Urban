using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreUI : MonoBehaviour
{ 
    [Header("카드 구역 연결")]
    [SerializeField] private Transform gridParent;   // 슬롯들이 생성될 그리드
    [SerializeField] private GameObject slotPrefab;  // 카드만 보여줄 슬롯 프리팹

    [Header("버튼 구역 연결 (수동 배치한 것들)")]
    // ★ 여기에 미리 만든 버튼 6개를 순서대로 넣으세요!
    [SerializeField] private Button[] buyButtons;
    [SerializeField] private TextMeshProUGUI[] buttonTexts; // 버튼 안의 텍스트들도 순서대로!

    [Header("UI 표시")]
    [SerializeField] private TextMeshProUGUI coinText;

    [Header("구매 팝업")]
    [SerializeField] private GameObject confirmPopup;
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("덱 확인 기능")]
    [SerializeField] private Button deckCheckButton; // 덱 확인버튼
    [SerializeField] private DeckCheckUI deckCheckUI; // 아까 만든 팝업 스크립트

    // 구매 진행 중 임시 저장 변수
    private Card targetCard;           // 살 카드 정보
    private StoreCardSlot targetSlot;  // 그 카드가 있는 슬롯 (회색으로 바꾸려고)
    private int targetIndex;           // 몇 번째 버튼인지 (버튼 끄려고)


    private List<StoreCardSlot> spawnedSlots = new List<StoreCardSlot>(); // 생성된 슬롯들 관리

    private void Start()
    {
        // 시작 시 테스트
        UpdateCoinUI();
        // OpenStore(); 
    }

    public void OpenStore()
    {
        gameObject.SetActive(true);
        InitStore();
    }

    private void InitStore()
    {
        UpdateCoinUI();

        // 1. 기존 슬롯 다 지우기 & 리스트 초기화
        foreach (Transform child in gridParent) Destroy(child.gameObject);
        spawnedSlots.Clear();

        // 2. 카드 가져오기
        if (CardFactory.Instance == null) return;
        List<Card> shopCards = CardFactory.Instance.GetRandomCards(6);

        // 3. 카드 슬롯 생성 및 버튼 연결
        for (int i = 0; i < 6; i++)
        {
            // 카드 슬롯 생성
            if (i < shopCards.Count)
            {
                GameObject slotObj = Instantiate(slotPrefab, gridParent);
                StoreCardSlot slotScript = slotObj.GetComponent<StoreCardSlot>();
                slotScript.Setup(shopCards[i]); // 카드 그림만 세팅
                spawnedSlots.Add(slotScript);   // 리스트에 저장해둠
            }

            // 버튼 세팅
            if (i < shopCards.Count)
            {
                Card cardData = shopCards[i]; // 이 버튼이 담당할 카드

                // 버튼 텍스트 가격으로 변경
                buttonTexts[i].text = $"{cardData.Price} G";
                buyButtons[i].interactable = true; // 버튼 활성화

                // 클릭 이벤트 연결
                int index = i;
                buyButtons[i].onClick.RemoveAllListeners();
                buyButtons[i].onClick.AddListener(() => OnClickBuyButton(cardData, index));
            }
            else
            {
                // 카드가 6장보다 적으면 남는 버튼은 꺼두기
                buyButtons[i].gameObject.SetActive(false);
            }
            deckCheckButton.onClick.AddListener(() => deckCheckUI.OpenDeckView());
        }

        // 팝업 초기화
        confirmPopup.SetActive(false);
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(OnBuyConfirmed);
        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(() => confirmPopup.SetActive(false));
    }

    private void UpdateCoinUI()
    {        
        coinText.text = $"{GameManager.Instance.CurrentCoin} G";
    }

    // 버튼을 눌렀을 때 (팝업 띄우기)
    private void OnClickBuyButton(Card card, int index)
    {
        targetCard = card;
        targetIndex = index;
        targetSlot = spawnedSlots[index]; // 해당 위치의 슬롯 스크립트 가져오기

        // 팝업 텍스트 설정
        popupText.text = $"\"{card.Name}\"\n{card.Price} G\n buy?";

        confirmPopup.SetActive(true);
        confirmPopup.transform.SetAsLastSibling();

        //confirmPopup.transform.localPosition = new Vector3(0, 0, -500f);
    }

    // 눌렀을 때 (실제 구매)
    private void OnBuyConfirmed()
    {
        // GameManager한테 카드구매요청
        if (GameManager.Instance != null && GameManager.Instance.UseCoin(targetCard.Price))
        {
            Debug.Log($" 카드구매성공 (-{targetCard.Price} G)");

            // 1. 덱(GameManager)에 카드 추가
            GameManager.Instance.AddCardToGlobalDeck(targetCard.Name); 

            // 2. UI 매진 처리
            buyButtons[targetIndex].interactable = false;
            buttonTexts[targetIndex].text = "Sold Out";
            if (targetSlot != null) targetSlot.SetSoldOutVisual();
            
            // 3. 팝업 닫기
            confirmPopup.SetActive(false);
        }
        else
        {
            // 돈이 부족하거나 GameManager가 없을 때
            Debug.Log($"결제 실패! (내 돈: {GameManager.Instance.CurrentCoin} / 가격: {targetCard.Price})");
            confirmPopup.SetActive(false);
        }
    }
}
