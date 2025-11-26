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
        player = GameManager.Instacne.Player;

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

        isPlayerTurn = true;
    }
}
