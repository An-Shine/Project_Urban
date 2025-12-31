abstract public class StatusEffect
{
    private readonly int initTurn;
    private int turn;
    public int RemainingTurn => turn;

    public StatusEffect(int initTurn)
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

    abstract public void Apply(Target target);
}