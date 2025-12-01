using UnityEngine;
public abstract class Defense : Card
{
    [SerializeField] protected int armor;

    public override CardType Type => CardType.Defense;
}