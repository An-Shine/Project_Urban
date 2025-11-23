using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BattleManager : SceneSingleton<BattleManager>
{
    // Player
    private int curTurn;
    private bool isPlayerTurn;
    private Player player;

    public int CurrentTurn => curTurn;
    public UnityEvent OnBattleEnd = new();

    private void Awake()
    {
        player = GameManager.Instacne.Player;
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
