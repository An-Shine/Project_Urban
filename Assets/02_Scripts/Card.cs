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

[System.Serializable]
public abstract class Card : MonoBehaviour
{
    //카드 기본 정보
    private string cardName;      // 카드 이름
    private CardType cardType;    // 카드 유형 
    private Sprite cardImage;     // (나중에 추가될 카드 이미지)

    
    // 특수 카드 여부
    public bool isSkillCard { get; }

    //제외 카드 여부
    public bool isExceptionCard { get; }
}