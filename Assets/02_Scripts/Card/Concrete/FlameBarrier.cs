using UnityEngine;

public class FlameBarrier : Defense 
{    
    public override CardName Name => CardName.FlameBarrier;
    public override int Use(Target target)
    {
        target.AddProtect(armor);
        target.Element = Element.Flame;

        return cost;
    }
}