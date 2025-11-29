public class TurnController : PointController
{
    protected override void ResetPoint()
    {
        curPoint = maxPoint;
    }
}