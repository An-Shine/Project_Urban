using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HpController))]
[RequireComponent(typeof(ShieldController))]
public class Enemy : Target
{   
    [Header("UI")]
    [SerializeField] HpBar hpBar;
    [SerializeField] Image selectedArrow;

    private void Awake()
    {
        hpController = GetComponent<HpController>();
        shieldController = GetComponent<ShieldController>();

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