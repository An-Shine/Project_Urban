public class ShieldController : PointController
{
    public override void ResetPoint()
    {
        curPoint = minPoint;
    }
}