using UnityEngine;
using UnityEngine.Events;

public class BattleManager : SceneSingleton<BattleManager>
{
    private int curTurn;
    private bool isPlayerTurn;
    private Player player;

    public int CurrentTurn => curTurn;
    public UnityEvent OnBattleEnd = new();

    private void Awake()
    {
        // GameManager에서 플레이어 참조 들고오고 플레이어 초기화
        player.Reset();
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
