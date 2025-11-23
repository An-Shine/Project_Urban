class CostController : PointController
{
    public override void ResetPoint()
    {
        curPoint = maxPoint;
    }
}