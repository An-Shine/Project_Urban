using UnityEngine;

public enum CardType
{
    Attack,  // 공격
    Defense, // 방어
    Buff,    // 버프
    Debuff   // 디버프
}
public enum CardElement
{
    None,       //무속성
    Flame,      //불꽃
    Ice,        //얼음
    Grass       //풀
}

// Prefab
abstract public class Card : MonoBehaviour
{
    //카드 기본 정보
    [SerializeField] protected Sprite cardImage;     // (나중에 추가될 카드 이미지)
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