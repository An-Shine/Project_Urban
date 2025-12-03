using UnityEngine;
using UnityEngine.UI;

public class Enemy : Target
{   
    [Header("UI")]
    [SerializeField] HpBar hpBar;
    [SerializeField] Image selectedArrow;
    
    protected override void Init()
    {
        base.Init();
        hpBar.Init(hpController);
        selectedArrow.enabled = false;
    }

    public void Attack(Player player)
    {
        player.Damage(1);
    }

    public void Hover()
    {
        selectedArrow.enabled = true;
    }

    public void UnHover()
    {
        selectedArrow.enabled = false;
    }
}