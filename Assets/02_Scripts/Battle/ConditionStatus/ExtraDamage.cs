[System.Serializable]
public class ExtraDamage : ConditionStatus
{    
    private readonly int damage;

    public ExtraDamage(int initTurn, int damage) : base(initTurn)
    {
        this.damage = damage;
    }

    public override void Execute(Target target)
    {
        DecreaseTurn();
    }
}