public class ScrapShield : EnemyAction, IExecute
{
    private readonly int shield = 15;
    private readonly int remainingTurn = 2;
    public override ActionType ActionType => ActionType.Defense;
    public override Element Element => Element.None;

    public bool CanExecute(Target target)
    {
        return target is Enemy;
    }

    public void Execute(Target target)
    {
        target.AddShield(shield);
        target.AddConditionStatus(new ArmorBreak(remainingTurn));
    }
}