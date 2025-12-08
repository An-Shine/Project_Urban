using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : SceneSingleton<BattleManager>
{
    // Player
    [SerializeField] private Player player;
    private int curTurn;
    private bool isPlayerTurn = true;
    private int earnedCoin;

    public Player Player => player;
    public int CurrentTurn => curTurn;
    public bool IsPlayerTurn => isPlayerTurn;

    // Event
    public UnityEvent OnTurnStart = new();
    public UnityEvent OnTurnEnd = new();
    public UnityEvent OnBattleEnd = new();
    
    private void Start()
    {
        OnBattleEnd.AddListener(HandleBattleEnd);
        OnTurnStart?.Invoke();
    }

    private void HandleBattleEnd()
    {
        // 보상 UI 활성화하기
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
