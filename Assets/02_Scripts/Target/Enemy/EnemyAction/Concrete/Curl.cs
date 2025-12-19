using UnityEngine;

[CreateAssetMenu(fileName = "Curl", menuName = "Enemy/Actions/Curl")]
public class Curl : EnemyAction
{
    [SerializeField] private int shield = 7;
    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.AddProtect(shield);
    }
}