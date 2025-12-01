using UnityEngine;

public class ElasticBarrier : Defense
{
    public override CardName Name => CardName.ElasticBarrier;

    public override int Use(Target target)
    {
        // 1. 방어도 추가
        target.AddShield(armor);

        // 2. 속성 변경
        if (target is Player player)
        {
            player.ChangeElement(Element.Grass);           
        }

        return cost;
    }
}