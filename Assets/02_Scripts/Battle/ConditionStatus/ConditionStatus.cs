abstract public class ConditionStatus
{
    private readonly int initTurn;
    private readonly TurnController turnController = new();
    public int RemainingTurn => turnController.CurrentPoint;

    public ConditionStatus(int initTurn)
    {
        this.initTurn = initTurn;

        turnController.MaxPoint = initTurn;
        turnController.CurrentPoint = initTurn;
    }

    public void DecreaseTurn(int amount = 1)
    {
        turnController.Decrease(amount);
    }

    public void IncreaseTurn(int amount = 1)
    {
        turnController.Increase(amount);
    }

    public void RefreshTurn()
    {
        turnController.CurrentPoint = initTurn;   
    }

    abstract public void Execute(Target target);
}