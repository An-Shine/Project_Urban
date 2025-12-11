using UnityEngine;

public class Player : Target
{
    [Header("Player Settings")]
    [SerializeField] private int maxCost;
    [SerializeField] private int startingDrawCount = 6; //시작할때 카드 6장 드로우

    [Header("Player View")]
    [SerializeField] protected CostView costView;

    [Header("Mouse Pointer")]
    [SerializeField] private MousePointer mousePointer;

    private Card prevSelectedCard;
    private Deck deck;

    public CostController Cost { get; private set; }
    public Deck Deck 
    {
        get 
        {
            deck ??= GameManager.Instance.Deck;
                
            return deck;
        }
    }

    private void Awake()
    {
        // Card Event
        mousePointer.OnCardSelect.AddListener(HandleCardSelect);
        mousePointer.OnCardUnSelect.AddListener(HandleCardUnSelect);
        mousePointer.OnCardEnter.AddListener(HandleCardEnter);
        mousePointer.OnCardExit.AddListener(HandleCardExit);

        // Enemy Event
        mousePointer.OnEnemySelect.AddListener(HandleEnemySelect);
        mousePointer.OnEnemyEnter.AddListener(HandleEnemyEnter);
        mousePointer.OnEnemyExit.AddListener(HandleEnemyExit);

        Hp = PointControllerFactory.CreateHp(maxHp);
        Protect = PointControllerFactory.CreateProtect(maxProtect);
        Cost = PointControllerFactory.CreateCost(maxCost);

        // 죽으면 종료처리
        OnDead.AddListener(HandleDead);
    }

    private void Start()
    {
        // Turn Event
        BattleManager.Instance.OnTurnStart.AddListener(HandleTurnStart);
        BattleManager.Instance.OnTurnEnd.AddListener(HandleTurnEnd);

        hpView.Bind(Hp);
        protectView.Bind(Protect);
        costView.Bind(Cost);
    }

    private void HandleTurnStart()
    {
        if (IsStun())
            BattleManager.Instance.TurnEnd();

        Deck.Draw(startingDrawCount);
    }

    private void HandleTurnEnd()
    {
        Deck.DiscardAll();
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
                Deck.Discard(card);
                mousePointer.ClearSelection();
                prevSelectedCard = null;

                Debug.Log($"Use Card To Player : {card.name}");
                Debug.Log($"Cur Shield : {Protect.CurrentPoint}");

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
        Deck.Discard(selectedCard);
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

    private void HandleDead(Target target)
    {
        BattleManager.Instance.OnBattleEnd.Invoke(false);
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
    }

    public void DrawCard(int amount)
    {
        Deck.Draw(amount);
    }
}
