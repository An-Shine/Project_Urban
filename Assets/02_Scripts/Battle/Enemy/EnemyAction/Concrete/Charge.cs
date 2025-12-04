public class Charge : EnemyAction, IExecute
{
    private readonly int damage = 6;

    public override ActionType ActionType => ActionType.Attack;

    public override Element Element => Element.None;

    public bool CanExecute(Target target)
    {
        return target is Player;
    }

    public void Execute(Target target)
    {
        target.Damage(damage);
    }
}