using UnityEngine;
abstract class Buff : Card
{
    [SerializeField] protected int turns;
    public override CardType Type => CardType.Buff;
}