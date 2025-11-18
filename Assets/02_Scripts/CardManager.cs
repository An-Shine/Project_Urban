using UnityEngine;

public enum CardType
{
    Attack,  // 공격
    Defense, // 방어
    Buff,    // 버프
    Debuff   // 디버프
}
[System.Serializable]
public abstract class Card
{
    //카드 기본 정보
    public string cardName;      // 카드 이름
    public CardType cardType;    // 카드 유형 
    public Sprite cardImage;     // (나중에 추가될 카드 이미지)

    // 3. 특수 카드 여부
    public bool skillCard { get; set; }

    //카드 효과 수치
    // 4. 공격 카드 데미지
    public int cardDamage;

    // 5. 방어 카드 방어도
    public int cardShield;

    // 6. 버프 수치
    public int buffEffect;

    // 7. 디버프 수치
    public int debuffEffect;

    /// <summary>
    /// 생성자: 카드를 생성할 때 기본 이름을 설정합니다.
    /// </summary>
    public Card(string name, CardType type)
    {
        this.cardName = name;
        this.cardType = type;
    }       
}