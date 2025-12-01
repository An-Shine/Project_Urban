using UnityEngine;

public class IceShield : Defense
{
    public override CardName Name => CardName.IceShield;

    public override int Use(Target target)
    {
        // 1. 방어도 추가
        target.AddShield(armor);

        // 2. 속성 변경
        if (target is Player player)
        {
            player.ChangeElement(Element.Ice);            
        }

        return cost;
    }
}