[System.Serializable]
public class Burn : ConditionStatus
{    
    public Burn(int initTurn) : base(initTurn) {}

    public override void Execute(Target target)
    {
        DecreaseTurn();
    }
}