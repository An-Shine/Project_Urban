using UnityEngine;

public class DoubleEdgedSword : Attack
{    
    [SerializeField] private int selfDamage = 1; 
    public override CardName Name => CardName.DoubleEdgedSword;

    public override int Use(Target target)
    {             
        target.Damage(damage);
        
        Player player = GameManager.Instance.Player;
        
        if (player != null)
        {            
            player.Damage(selfDamage);
        }
        
        return cost;
    }
}