using UnityEngine;

public class Card2 : DefenseCard
{
    public override int Use(Target target)
    {
        target.AddShield(shield);

        return cost;
    }
}