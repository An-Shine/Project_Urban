class Ambush : Attack
{
    public override CardName Name => CardName.Ambush;

    public override int Use(Target target)
    {
        target.Damage(damage);

        return cost;
    }
}