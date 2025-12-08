[System.Serializable]
public class Broken : ConditionStatus
{    
    public Broken(int initTurn) : base(initTurn) {}

    public override void Execute(Target target)
    {
        DecreaseTurn();
    }
}