public enum ActionType
{
    Attack,  // 공격
    Defense, // 방어
    Buff,    // 버프
    Debuff   // 디버프
}


abstract public class EnemyAction
{
    abstract public ActionType ActionType { get; }
    abstract public Element Element { get; }
} 