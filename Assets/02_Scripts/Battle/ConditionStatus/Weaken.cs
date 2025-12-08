[System.Serializable]
public class Weaken : ConditionStatus
{    
    public Weaken(int initTurn) : base(initTurn) {}

    public override void Execute(Target target)
    {
        DecreaseTurn();
    }
}