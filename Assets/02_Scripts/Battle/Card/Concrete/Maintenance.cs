using UnityEngine;

public class Maintenance : Card
{     
    [SerializeField] private int drawCount = 2;
    public override CardName Name => CardName.Maintenance;
    
    public override int Use(Target target)
    {       
        if (target is Player player)
        {
            player.DrawCards(drawCount);
        }       
        
        return cost;
    }
}