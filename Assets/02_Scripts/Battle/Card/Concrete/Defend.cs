class Defend : Defense
{
    public override CardName Name => CardName.Defend;

    public override int Use(Target target)
    {
        target.AddShield(armor);
        return cost;
    }
}