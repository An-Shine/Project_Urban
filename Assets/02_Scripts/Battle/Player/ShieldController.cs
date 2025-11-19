public class ShieldController : PointController
{
    protected override void ResetPoint()
    {
        curPoint = minPoint;
    }
}