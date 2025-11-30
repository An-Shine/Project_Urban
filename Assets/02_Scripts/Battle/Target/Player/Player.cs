using System;
using UnityEngine;

[RequireComponent(typeof(CostController))]
[RequireComponent(typeof(Deck))]
public class Player : Target
{
    // UI
    [SerializeField] private CostText costText;
    [SerializeField] private HpBar hpBar;
    [SerializeField] private int startingDrawCount = 6; //시작할때 카드 6장 드로우

    private Deck deck;

    // Component
    private CostController costController;
    public CostController CostController => costController;

    // Card
    private Card selectedCard;
    private Vector3 originPos;

    // Target의 Awake에서 호출
    protected override void Init()
    {
        base.Init();
        hpController.Reset();

        costController = GetComponent<CostController>();
        deck = GetComponent<Deck>();
        
        if (hpBar != null) hpBar.Init(hpController);
        if (costText != null) costText.Init(costController);
        if(deck != null) deck.Init();

        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnTurnEnd.AddListener(HandleTurnEnd);
        }
    }

    private void HandleTurnEnd()
    {        
        if (deck != null)
        {
            deck.DiscardHand();
        }            
    }

    // Target의 Awake에서 호출 -> BattleManager.StartBattle에서 호출
    public override void Reset()
    {        
        shieldController.Reset();
        costController.Reset();

        // 덱이 준비되었을 때만 드로우
        if (deck != null)
        {
            // 뽑기 전에 덱을 리셋해야 추가된 카드가 반영
            deck.ResetDeck(); 
            
            // 그 다음 뽑기
            deck.DrawCard(startingDrawCount);   
        }
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
                   
                    if (deck != null)
                    {
                        deck.Discard(selectedCard);
                    }

                    selectedCard = null;
                }                
                gameObject.transform.localScale /= 1.25f;
            }
        });
    }

    private void Update()
    {        
        if (BattleManager.Instance == null) return;

        if (!BattleManager.Instance.IsPlayerTurn) return;

        if (selectedCard != null)
        {
            if (Camera.main != null)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = -1;
                selectedCard.transform.position = mousePos;
            }
        }
    }    
    internal void OnPlayerTurnEnd()
    {
    }
       
}