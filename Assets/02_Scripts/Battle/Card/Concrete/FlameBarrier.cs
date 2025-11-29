using UnityEngine;

class FlameBarrier : Defense
{
    // CardName Enum에 정의된 FlameBarrier 사용
    public override CardName Name => CardName.FlameBarrier;

    private void Awake()
    {       
        this.armor = 0; 
    }

    public override int Use(Target target)
    {
        // 1. 방어도 추가 
        target.AddShield(armor);

        // 2. 속성 변경 (Player인지 확인 후 변경)    
        if (target is Player player)
        {
            // 얼음속성으로 변경            
            player.ChangeElement(Element.Flame);
        }

        return cost;
    }
}