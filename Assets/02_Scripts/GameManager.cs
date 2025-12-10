using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class GameManager : Singleton<GameManager>
{
    public static new GameManager Instance;
    [SerializeField] private int coin = 500;
    public int CurrentCoin => coin;
    [SerializeField] private TextMeshProUGUI coinText;
    public List<CardName> SelectedBonusCards { get; private set; } = new List<CardName>();

    //배틀씬과 메인씬간에 덱 정보 주고받기 위함
    public List<CardName> GlobalDeck = new List<CardName>();
    

    public void Start()
    {
        UpdateCoinText();
    }
    private void Awake()
    {
        // 싱글톤 패턴 (중복 방지 + 씬 이동 시 유지)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ★ 씬이 바껴도 파괴되지 않음!
        }
        else
        {
            Destroy(gameObject);
        }
        
        
    }

    // 카드를 저장하는 함수
    public void SetBonusCards(CardName card1, CardName card2)
    {
        SelectedBonusCards.Clear();
        SelectedBonusCards.Add(card1);
        SelectedBonusCards.Add(card2);
    }

    //플레이어가 처음에 선택한 속성 저장(StoreUI 용)
    public Element SelectedElement { get; set; } = Element.None;

    // 메인화면으로 돌아갈 때, 맵 선택창을 띄울지 여부
    public bool IsStageSelectMode { get; set; } = false;

    public void AddCoin(int amount)
    {
        coin += amount;
    }

    //코인 사용관련 함수
    public bool UseCoin(int amount)
    {
        if (coin >= amount)
        {
            coin -= amount;
            UpdateCoinText();
            return true;
        }
        return false;
    }

    public void SetCoinText(TextMeshProUGUI newText)
    {
        coinText = newText;
        UpdateCoinText(); // 연결되자마자 금액 한번 갱신해주기
    }
    private void UpdateCoinText()
    {
        if (coinText != null) coinText.text = $"{coin} G";
    }

    // 덱관리 함수
    // 상점에서 카드 하나 추가할 때
    public void AddCardToGlobalDeck(CardName cardName)
    {
        GlobalDeck.Add(cardName);
    }

    // (Deck에서 호출) 초기 덱을 덮어씌울 때
    public void SetInitialDeck(List<CardName> starterDeck)
    {
        GlobalDeck.Clear();
        GlobalDeck.AddRange(starterDeck);
    }
    
    // 3. 덱 가져오기 (배틀 시작할 때 호출)
    public List<CardName> GetGlobalDeck()
    {
        return GlobalDeck;
    }
}