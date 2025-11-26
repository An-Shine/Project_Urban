using UnityEngine;
abstract class Attack : Card
{
    [SerializeField] protected int damage;
    public override CardType Type => CardType.Attack;
}