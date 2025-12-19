abstract public class ConditionStatus
{
    private readonly int initTurn;
    private int turn;
    public int RemainingTurn => turn;

    public ConditionStatus(int initTurn)
    {
        this.initTurn = initTurn;

    }

    public void DecreaseTurn(int amount = 1)
    {
        turn -= amount;
    }

    public void IncreaseTurn(int amount = 1)
    {
        turn += amount;
    }

    public void RefreshTurn()
    {
        turn = initTurn;   
    }

    abstract public void Execute(Target target);
}