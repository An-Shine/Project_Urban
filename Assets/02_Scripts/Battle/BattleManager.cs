using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : SceneSingleton<BattleManager>
{
    // Player
    private int curTurn;
    private bool isPlayerTurn = true;
    private Player player;

    [SerializeField] private Transform enemyZone;
    [SerializeField] private Enemy enemyPrefab;
    private readonly Enemy [] enemies = new Enemy[3];

    public int CurrentTurn => curTurn;
    public bool IsPlayerTurn => isPlayerTurn;
    public UnityEvent OnBattleEnd = new();

    private void Start()
    {
        player = GameManager.Instance.Player;

        float enemyXpos = -3.5f;
        for (int  i = 0 ; i < enemies.Length; i++)
        {
            enemies[i] = Instantiate(
                enemyPrefab,
                enemyZone.position + new Vector3(enemyXpos, 0, 0),
                Quaternion.identity,
                enemyZone
            );

            enemyXpos += 3.5f;
        }
    }

    public void TurnEnd()
    {
        player.OnPlayerTurnEnd();

        isPlayerTurn = false;
        curTurn++;

        ExecuteEnemyAction();
    }

    private void ExecuteEnemyAction()
    {
        StartCoroutine(ExecuteEnemyActionRoutine());
    }

    private IEnumerator ExecuteEnemyActionRoutine()
    {
        for (int i = 0 ; i < enemies.Length; i++)
        {
            enemies[i].Attack(player);
            enemies[i].transform.localScale *= 1.2f;
            yield return new WaitForSeconds(0.5f);
            enemies[i].transform.localScale /= 1.2f;
        }
        //적 턴 종료 시 도트데미지 적용 
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null && enemies[i].HpController.CurrentPoint > 0)
            {
                enemies[i].OnTurnEnd();
            }
        }
        yield return new WaitForSeconds(0.5f);
        
        player.Reset();
        isPlayerTurn = true;
    }
}
