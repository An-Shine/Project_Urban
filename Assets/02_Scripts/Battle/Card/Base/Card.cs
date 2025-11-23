using UnityEngine;

public enum CardType
{
    AttackCard,  // 공격
    DefenseCard, // 방어
    BuffCard,    // 버프
    DebuffCard   // 디버프
}
public enum CardElement
{
    None,       //무속성
    Flame,      //불꽃
    Ice,        //얼음
    Grass       //풀
}

// Prefab
public abstract class Card : MonoBehaviour
{
    //카드 기본 정보
    [SerializeField] protected string cardName;      // 카드 이름
    [SerializeField] protected CardType cardType;    // 카드 유형 
    [SerializeField] protected Sprite cardImage;     // (나중에 추가될 카드 이미지)
    [SerializeField] protected int cost;            // 코스트
    
    // 특수 카드 여부
    public bool IsSpecial { get; }

    //제외 카드 여부
    public bool IsException { get; }

    public int Cost => cost;

    abstract public int Use(Target target);
}