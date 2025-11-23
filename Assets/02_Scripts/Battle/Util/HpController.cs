public class HpController : PointController
{
    public override void ResetPoint()
    {
        curPoint = maxPoint;
    }
}
