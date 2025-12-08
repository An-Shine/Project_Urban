[System.Serializable]
public class Armor : ConditionStatus
{    
    public Armor(int initTurn) : base(initTurn) {}

    public override void Execute(Target target)
    {
        DecreaseTurn();
    }
}