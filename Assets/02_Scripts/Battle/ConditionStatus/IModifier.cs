public interface IModifier
{
    int ModifyDamage(int baseDamage);
    int ModifyCost(int baseCost);
    int ModifyShield(int baseShield);
    Element ModifyElement(Element baseElement);
}