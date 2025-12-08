using UnityEngine;

[CreateAssetMenu(fileName = "Screw", menuName = "Enemy/Actions/Screw")]
public class Screw : EnemyAction
{
    [SerializeField] private int damage = 5;
    [SerializeField] private int count = 2;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        for (int i = 0; i < count; i++)
            target.Damage(damage);
    }
}