using UnityEngine;

public class Enemy : Target
{
    [SerializeField] HpBar hpBar;
    
    protected override void Init()
    {
        base.Init();
        hpBar.Init(hpController);
    }

    public void Attack(Player player)
    {
        player.Damage(1);
    }
}