[System.Serializable]
public class FrozenStatus : ConditionStatus
{    
    public FrozenStatus(int initTurn) : base(initTurn)
    {
        
    }
    public override void Execute(Target target)
    {
        // 빙결은 데미지를 주지 않고, 턴 수만 차감        
        DecreaseTurn();
    }
}