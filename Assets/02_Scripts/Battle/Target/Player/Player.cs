using UnityEngine;

[RequireComponent(typeof(CostController))]
[RequireComponent(typeof(Deck))]
public class Player : Target
{
    // UI
    [SerializeField] private CostText costText;
    [SerializeField] private HpBar hpBar;
    [SerializeField] private int startingDrawCount = 6; //시작할때 카드 6장 드로우

    // Deck Component
    private Deck deck;

    // Component
    private CostController costController;
    public CostController CostController => costController;

    // Card
    private Card selectedCard;
    private Vector3 originPos;

    public Element ShieldElement { get; set; } = Element.None;

    // Target의 Awake에서 호출
    protected override void Init()
    {
        base.Init();
        hpController.Reset();

        costController = GetComponent<CostController>();
        deck = GetComponent<Deck>();

        deck.Init();
        hpBar.Init(hpController);
        costText.Init(costController);

        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnTurnEnd.AddListener(HandleTurnEnd);
        }
    }

    private void HandleTurnEnd()
    {        
        if (deck != null)
        {
            // 남은 카드를 모두 무덤으로 날려보냄
            deck.DiscardHand();
        }            
    }

    // Target의 Awake에서 호출
    public override void Reset()
    {
        // 새로운 전투 시작 시, HP는 리셋하지 않는다
        shieldController.Reset();
        costController.Reset();

        deck.DrawCard(startingDrawCount);   //설정된 갯수만큼 드로우
    }

    public void DrawCard(int amount)
    {
        deck.DrawCard(amount);
    }

    protected override void Start()
    {
        base.Start();

        MouseEvents.OnMouseDown.AddListener((gameObject) =>
        {
            if (gameObject.CompareTag("Card"))
            {
                selectedCard = gameObject.GetComponent<Card>();
                originPos = gameObject.transform.position;
            }
        });

        MouseEvents.OnMouseUp.AddListener((gameObject) =>
        {
            if (selectedCard != null && gameObject.CompareTag("Card"))
            {
                selectedCard.transform.position = originPos;
                originPos = Vector3.zero;
                selectedCard = null;
            }

        });

        MouseEvents.OnMouseEnter.AddListener((gameObject) =>
        {
            if (selectedCard != null && gameObject.CompareTag("Enemy"))
            {
                gameObject.transform.localScale *= 1.25f;
            }
        });

        MouseEvents.OnMouseExit.AddListener((gameObject) =>
        {
            if (selectedCard != null && gameObject.CompareTag("Enemy"))
            {
                gameObject.transform.localScale /= 1.25f;
            }
        });

        MouseEvents.OnMouseUp.AddListener((gameObject) =>
        {
            if (selectedCard != null && gameObject.CompareTag("Enemy"))
            {
                if (selectedCard.Cost <= costController.CurrentPoint)
                {
                    int cost = selectedCard.Use(gameObject.GetComponent<Target>());
                    costController.Decrease(cost);

                    //핸드에서 카드 삭제
                    if (deck != null)
                    {
                        deck.Discard(selectedCard);
                    }

                    selectedCard = null;
                }

                gameObject.transform.localScale /= 1.25f;
            }
        });

        BattleManager.Instance.OnTurnEnd.AddListener(() => deck.DiscardHand());
    }

    private void Update()
    {
        if (!BattleManager.Instance.IsPlayerTurn)
            return;

        if (selectedCard != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = -1;

            selectedCard.transform.position = mousePos;
        }
    }

}
