using UnityEngine;

public class Ignite : Debuff
{   
    [SerializeField] private int damage = 5;
    [SerializeField] private int burnDamage = 1; // 도트 데미지
    [SerializeField] private int duration = 3;

    public override CardName Name => CardName.Ignite;

    public override int Use(Target target)
    {
        // 1. 즉발 데미지 
        target.Damage(damage);

        // 2. 도트 데미지 
        target.AddDoT(burnDamage, duration);
        
        return cost;
    }
}