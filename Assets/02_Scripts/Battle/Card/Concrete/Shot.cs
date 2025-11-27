class Shot : Attack
{
    public override CardName Name => CardName.Shot;

    public override int Use(Target target)
    {
        target.Damage(damage);

        return cost;
    }
}