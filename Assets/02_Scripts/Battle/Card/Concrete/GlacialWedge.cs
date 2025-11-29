using UnityEngine;

public class GlacialWedge : Debuff
{    
    [SerializeField] private int freezeTurn = 1;
    [SerializeField] private int damage = 0;
    
    public override CardName Name => CardName.GlacialWedge;

    public override int Use(Target target)
    {
        
        target.Damage(damage);

        // 빙결 상태 적용         
        target.AddFreeze(freezeTurn);
       
        return cost;
    }
}