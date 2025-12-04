using UnityEngine;

[RequireComponent(typeof(HpController))]
[RequireComponent(typeof(ShieldController))]
[RequireComponent(typeof(CostController))]
public class Player : Target
{
    [Header("Initial Settings")]
    [SerializeField] private int startingDrawCount = 6; //시작할때 카드 6장 드로우

    // UI
    [Header("UI Object")]
    [SerializeField] private CostText costText;
    [SerializeField] private HpBar hpBar;

    [Header("Mouse Pointer Object")]
    [SerializeField] private MousePointer mousePointer;

    // Deck Component
    [SerializeField] private Deck deck;
    private Card prevSelectedCard;

    // Component
    private CostController costController;
    public CostController CostController => costController;

    public Element ShieldElement { get; set; } = Element.None;
    public Deck Deck => deck;

    private void Awake()
    {
        hpController = GetComponent<HpController>();
        shieldController = GetComponent<ShieldController>();
        costController = GetComponent<CostController>();

        // UI Init
        hpBar.Init(hpController);
        costText.Init(costController);
    }

    private void Start()
    {
        // Turn Event
        BattleManager.Instance.OnTurnStart.AddListener(HandleTurnStart);
        BattleManager.Instance.OnTurnEnd.AddListener(HandleTurnEnd);

        // Card Event
        mousePointer.OnCardSelect.AddListener(HandleCardSelect);
        mousePointer.OnCardUnSelect.AddListener(HandleCardUnSelect);
        mousePointer.OnCardEnter.AddListener(HandleCardEnter);
        mousePointer.OnCardExit.AddListener(HandleCardExit);

        // Enemy Event
        mousePointer.OnEnemySelect.AddListener(HandleEnemySelect);
        mousePointer.OnEnemyEnter.AddListener(HandleEnemyEnter);
        mousePointer.OnEnemyExit.AddListener(HandleEnemyExit);
    }

    private void HandleTurnStart()
    {
        if (IsStun())
            BattleManager.Instance.TurnEnd();

        deck.Draw(startingDrawCount);
    }

    private void HandleTurnEnd()
    {
        deck.DiscardAll();
        costController.Reset();
    }

    private void HandleCardSelect(Card card)
    {
        // 이미 선택된 카드를 다시 클릭한 경우
        if (prevSelectedCard == card)
        {
            // 자신에게 사용하는 카드인 경우, 선택된 카드를 다시 클릭하면 사용된다.
            if (card.Type == CardType.Defense || card.Type == CardType.Buff)
            {
                card.Use(this);
                deck.Discard(card);
                mousePointer.ClearSelection();
                prevSelectedCard = null;

                Debug.Log($"Use Card To Player : {card.name}");
                Debug.Log($"Cur Shield : {shieldController.CurrentPoint}");

                return;
            }

            // 전체 공격 혹은 전체 디버프 처리
            else if (card.Type == CardType.Attack || card.Type == CardType.Debuff)
            {
                return; // 아직 미처리
            }

            return;
        }

        // 새로운 카드 선택
        if (prevSelectedCard != null)
        {
            prevSelectedCard.UnHover();
            card.Hover();
        }

        prevSelectedCard = card;
    }

    private void HandleCardEnter(Card card)
    {
        if (prevSelectedCard == null)
            card.Hover();
    }

    private void HandleCardExit(Card card)
    {
        if (prevSelectedCard == null)
            card.UnHover();
    }

    private void HandleCardUnSelect(Card card)
    {
        Card selectedCard = mousePointer.SelectedCard;

        if (selectedCard != null)
            selectedCard.UnHover();

        mousePointer.ClearSelection();
        prevSelectedCard = null;
    }

    private void HandleEnemySelect(Enemy enemy)
    {
        if (!CanTargetEnemy())
            return;

        Card selectedCard = mousePointer.SelectedCard;
        selectedCard.Use(enemy);

        enemy.UnHover();
        deck.Discard(selectedCard);
        mousePointer.ClearSelection();
    }

    private void HandleEnemyEnter(Enemy enemy)
    {
        if (CanTargetEnemy())
            enemy.Hover();
    }

    private void HandleEnemyExit(Enemy enemy)
    {
        if (CanTargetEnemy())
            enemy.UnHover();
    }

    private bool CanTargetEnemy()
    {
        Card selectedCard = mousePointer.SelectedCard;
        if (selectedCard == null) return false;
        if (selectedCard.Type == CardType.Defense || selectedCard.Type == CardType.Buff) return false;

        return true;
    }

    public void Reset()
    {
        // 새로운 전투 시작 시, HP는 리셋하지 않는다
        shieldController.Reset();
        costController.Reset();
    }

    public void DrawCard(int amount)
    {
        deck.Draw(amount);
    }
}
