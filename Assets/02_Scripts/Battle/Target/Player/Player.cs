using UnityEngine;

[RequireComponent(typeof(CostController))]
public class Player : Target
{
    // UI
    [SerializeField] private CostText costText;
    [SerializeField] private HpBar hpBar;

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
        costController = GetComponent<CostController>();

        hpBar.Init(hpController);
        costText.Init(costController);
    }

    // Target의 Awake에서 호출
    protected override void Reset()
    {
        // 새로운 전투 시작 시, HP는 리셋하지 않는다
        shieldController.Reset();
        costController.Reset();
    }

    private void Start()
    {
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
                }
                
                gameObject.transform.localScale /= 1.25f;
            }
        });
    }

    private void Update()
    {
        if (selectedCard != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = -1;

            selectedCard.transform.position = mousePos;
        }
    }

}
