public class HpController : PointController
{
    protected override void ResetPoint()
    {
        curPoint = minPoint;
    }
}
