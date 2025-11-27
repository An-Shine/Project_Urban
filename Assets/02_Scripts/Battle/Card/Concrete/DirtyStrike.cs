class DirtyStrike : Attack
{
    public override CardName Name => CardName.DirtyStrike;

    public override int Use(Target target)
    {
        target.Damage(damage);

        return cost;
    }
}