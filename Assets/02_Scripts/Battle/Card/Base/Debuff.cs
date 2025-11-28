using UnityEngine;
abstract class Debuff : Card
{
    [SerializeField] protected int turns;
    public override CardType Type => CardType.Debuff;
}