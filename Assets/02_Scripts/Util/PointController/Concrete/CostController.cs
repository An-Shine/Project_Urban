public class CostController : PointController
{
    protected override void ResetPoint()
    {
        curPoint = maxPoint;
    }
}