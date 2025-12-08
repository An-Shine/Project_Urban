class Slash : Attack
{
    public override CardName Name => CardName.Slash;

    public override int Use(Target target)
    {
        target.Damage(damage);

        return cost;
    }
}