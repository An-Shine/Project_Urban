[System.Serializable]
public class Bleed : ConditionStatus
{    
    public Bleed(int initTurn) : base(initTurn) {}

    public override void Execute(Target target)
    {
        DecreaseTurn();
    }
}