using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : SceneSingleton<EnemyManager>
{
    static readonly float variationCoefficient = 0.2f;

    [SerializeField] private int enemyCount;
    [SerializeField] private float enemySpacing = 3.5f;
    [SerializeField] private List<Enemy> enemyPrefabs = new();

    private readonly List<Enemy> enemies = new();

    private void Start()
    {
        int prefabsCount = enemyPrefabs.Count;

        for (int i = 0; i < enemyCount; i++)
        {
            Enemy enemy = Instantiate(
                enemyPrefabs[Random.Range(0, prefabsCount)],
                transform.position,
                Quaternion.identity,
                transform
            );

            enemies.Add(enemy);
            enemy.OnDead.AddListener(HandleEnemyDead);
        }

        AlignEnemies();
    }

    private void AlignEnemies()
    {
        int count = enemies.Count;
        if (count == 0) return;

        float totalWidth = (count - 1) * enemySpacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < count; i++)
        {
            Vector3 targetPos = transform.position + new Vector3(startX + (i * enemySpacing), 0, 0);
            enemies[i].MoveTo(targetPos);
        }
    }

    private void HandleEnemyDead(Target target)
    {
        enemies.Remove(target as Enemy);
        Destroy(target.gameObject);
        AlignEnemies();

        if (enemies.Count == 0)
            BattleManager.Instance.OnBattleEnd?.Invoke();

        BattleManager.Instance.AddCoin(CalcCoin(target as Enemy));
    }

    public void ExecuteEnemyAction(UnityAction completeRoutine = null)
    {
        StartCoroutine(ExecuteEnemyActionRoutine(completeRoutine));
    }

    private IEnumerator ExecuteEnemyActionRoutine(UnityAction completeRoutine)
    {
        for (int i = 0 ; i < enemies.Count; i++)
        {   
            Enemy enemy = enemies[i];

            if (enemy.IsStun())
                continue;

            // Enemy Attack Animation 
            
            enemy.Action();

            // Enemy Anim
            enemy.transform.localScale *= 1.2f;
            yield return new WaitForSeconds(0.5f);
            enemy.transform.localScale /= 1.2f;
        }
        
        completeRoutine?.Invoke();   
    }

    private int CalcCoin(Enemy enemy)
    {
        return (int)(10.0f * (1.0f + enemy.RewardCoefficient) * (1.0f + Random.Range(variationCoefficient * -1.0f, variationCoefficient)));
    }
}