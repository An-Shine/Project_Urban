using UnityEngine;

[CreateAssetMenu(fileName = "CrudeMolotov", menuName = "Enemy/Actions/CrudeMolotov")]
public class CrudeMolotov : EnemyAction
{
    [SerializeField] private int damage = 4;
    [SerializeField] private int count = 2;
    [SerializeField] private int remainingTurn = int.MaxValue;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Flame;

    public override void Execute(Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(damage);
            
        // target.AddConditionStatus(new Burn(remainingTurn));
    }
}