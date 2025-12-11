using UnityEngine;
using UnityEngine.Events;

public class BattleManager : SceneSingleton<BattleManager>
{
    // Player
    [SerializeField] private Player player;
    private int curTurn;
    private bool isPlayerTurn = true;
    //private int earnedCoin;
    [SerializeField] private int earnedCoin = 500; // 초기값 (테스트용)
    public int CurrentCoin => earnedCoin;

    public Player Player => player;
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

    // 코인을 이용해서 카드 구매시도 함수
    public bool TrySpendCoin(int amount)
    {
        if (earnedCoin >= amount)
        {
            earnedCoin -= amount;
            Debug.Log($"{amount} 코인 사용, 남은 코인 : {earnedCoin}");
        }        
        return false;
    }
}
