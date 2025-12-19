using UnityEngine;

[CreateAssetMenu(fileName = "Charge", menuName = "Enemy/Actions/Charge")]
public class Charge : EnemyAction
{
    [SerializeField] private int damage = 6;

    public override ActionType Type => ActionType.Attack;

    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.Damage(damage);
    }
}