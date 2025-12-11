using UnityEngine;
using UnityEngine.Events;

public class BattleManager : SceneSingleton<BattleManager>
{
    // Player
    [SerializeField] private Player player;
    [SerializeField] private Hand hand;
    private int curTurn;
    private bool isPlayerTurn = true;
    private int earnedCoin;

    public Player Player => player;
    public Hand Hand => hand;
    public int CurrentTurn => curTurn;
    public bool IsPlayerTurn => isPlayerTurn;
    public int EarnedCoin => earnedCoin;

    // Event
    public UnityEvent OnTurnStart = new();
    public UnityEvent OnTurnEnd = new();
    public UnityEvent<bool> OnBattleEnd = new();
    
    private void Start()
    {
        OnBattleEnd.AddListener(HandleBattleEnd);
        OnTurnStart?.Invoke();
    }

    private void HandleBattleEnd(bool isSuccess)
    {
        Debug.Log("Battle End!");
        GameManager.Instance.AddCoin(earnedCoin);
    }

    // UI로 호출
    public void TurnEnd()
    {
        OnTurnEnd?.Invoke();

        isPlayerTurn = false;
        curTurn++;

        EnemyManager.Instance.ExecuteEnemyAction(() =>
        {
            isPlayerTurn = true;
            OnTurnStart?.Invoke();
        });
    }

    public void AddCoin(int amount)
    {
        earnedCoin += amount;
        Debug.Log($"Current Coin : {earnedCoin}");
    }
}
