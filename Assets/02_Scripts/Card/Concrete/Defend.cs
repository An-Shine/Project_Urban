class Defend : Defense
{
    public override CardName Name => CardName.Defend;

    public override int Use(Target target)
    {
        target.AddProtect(armor);
        return cost;
    }
}