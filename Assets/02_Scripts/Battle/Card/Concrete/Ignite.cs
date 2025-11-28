using UnityEngine;

public class Ignite : Card // (AttackCard 상속받아도 됨)
{   
    [SerializeField] private int initialDamage = 1; // 즉발 데미지
    [SerializeField] private int tickDamage = 1;    // 턴당 데미지
    [SerializeField] private int duration = 3;      // 지속 시간
    
    public override CardName Name => CardName.Ignite;

    public override int Use(Target target)
    {
        // 즉발 데미지
        target.Damage(initialDamage);

        // 도트데미지 예약 
        target.AddDoT(tickDamage, duration);       

        return cost;
    }
}