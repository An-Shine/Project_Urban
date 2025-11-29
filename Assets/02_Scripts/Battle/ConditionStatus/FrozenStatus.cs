[System.Serializable]
public class FrozenStatus : ConditionStatus
{
    public FrozenStatus(int initTurn) : base(initTurn)
    {

    }
    public override void Execute(Target target)
    {        
        DecreaseTurn();
    }
}