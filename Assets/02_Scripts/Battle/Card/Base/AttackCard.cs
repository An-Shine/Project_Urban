using UnityEngine;

public class AttackCard : Card
{
    [SerializeField] protected int damage;          // 데미지
    [SerializeField] protected int count = 1;           // 공격 횟수
    [SerializeField] protected CardElement element; // 속성

    public override int Use(Target target)
    {
        for (int i = 0; i < count; i++)
        {
            target.Damage(damage);
        }

        return cost;
    }
}