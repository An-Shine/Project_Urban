[System.Serializable]
public class ArmorBreak : ConditionStatus
{    
    public ArmorBreak(int initTurn) : base(initTurn) {}

    public override void Execute(Target target)
    {
        DecreaseTurn();
    }
}