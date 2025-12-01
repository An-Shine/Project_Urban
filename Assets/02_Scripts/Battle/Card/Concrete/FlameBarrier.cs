using UnityEngine;

public class FlameBarrier : Defense 
{    
    public override CardName Name => CardName.FlameBarrier;
    public override int Use(Target target)
    {
        // 방어도 추가
        target.AddShield(armor);

        // 속성 변경
        if (target is Player player)
            player.ShieldElement = Element.Flame;

        return cost;
    }
}