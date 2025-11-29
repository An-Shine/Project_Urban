using UnityEngine;

public enum CardType
{
    Attack,  // 공격
    Defense, // 방어
    Buff,    // 버프
    Debuff   // 디버프
}

// Prefab
abstract public class Card : MonoBehaviour
{
    //카드 기본 정보
    [SerializeField] protected Element element;
    [SerializeField] protected int cost;            // 코스트
    
    // 특수 카드 여부
    public bool IsSpecial { get; }

    //제외 카드 여부
    public bool IsException { get; }

    public int Cost => cost;

    abstract public CardName Name { get; }
    abstract public CardType Type { get;}
    abstract public int Use(Target target);
}