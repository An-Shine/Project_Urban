abstract public class ConditionStatus
{
    private readonly int initTurn;
    private readonly  TurnController turn;
    public int RemainingTurn => turn.CurrentPoint;

    public ConditionStatus(int initTurn)
    {
        this.initTurn = initTurn;
        turn = PointControllerFactory.CreateTurn(initTurn);
    }

    public void DecreaseTurn(int amount = 1)
    {
        turn.Decrease(amount);
    }

    public void IncreaseTurn(int amount = 1)
    {
        turn.Increase(amount);
    }

    public void RefreshTurn()
    {
        turn.CurrentPoint = initTurn;   
    }

    abstract public void Execute(Target target);
}