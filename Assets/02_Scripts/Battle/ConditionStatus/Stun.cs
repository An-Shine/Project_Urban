[System.Serializable]
public class Stun : ConditionStatus
{    
    public Stun(int initTurn) : base(initTurn) {}

    public override void Execute(Target target)
    {
        DecreaseTurn();
    }
}