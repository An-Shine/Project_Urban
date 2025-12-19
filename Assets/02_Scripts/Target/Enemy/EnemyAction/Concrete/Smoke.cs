using UnityEngine;

[CreateAssetMenu(fileName = "Smoke", menuName = "Enemy/Actions/Smoke")]
public class Smoke : EnemyAction
{
    [SerializeField] private int shield = 10;
    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.AddProtect(shield);
    }
}