class Guard : Defense
{
    public override CardName Name => CardName.Guard;

    public override int Use(Target target)
    {
        target.AddProtect(armor);
        return cost;
    }
}