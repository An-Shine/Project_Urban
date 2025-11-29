[System.Serializable]
public class DoTDamage : ConditionStatus
{
    private readonly int damage;   // 틱당 데미지
    public int Damage => damage;

    public DoTDamage(int damage, int initTurn) : base(initTurn)
    {
        this.damage = damage;  
    }

    public override void Execute(Target target)
    {
        target.Damage(damage);
        DecreaseTurn();
    }
}