using UnityEngine;
using UnityEngine.Events;

public class BattleManager : SceneSingleton<BattleManager>
{
    // Player
    private int curTurn;
    private bool isPlayerTurn;
    private Player player;

    // Card
    private Card selectedCard;
    private Vector3 originPos;


    public int CurrentTurn => curTurn;
    public UnityEvent OnBattleEnd = new();

    private void Awake()
    {
        player = GameManager.Instacne.Player;
        player.Reset();
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
                selectedCard.Use(gameObject.GetComponent<Target>());
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

    public void TurnEnd()
    {
        isPlayerTurn = false;
        curTurn++;

        ExecuteEnemyAction();
    }

    private void ExecuteEnemyAction()
    {
        // 코루틴 처리 후 isPlayerTurn = true로 변경
    }
}
