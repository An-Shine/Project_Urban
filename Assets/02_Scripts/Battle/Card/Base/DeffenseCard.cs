using UnityEngine;

abstract public class DefenseCard : Card
{
    //DefenseCard 데이터
    [SerializeField] protected int shield;           // 방어도 (Damage 대신 사용)
    [SerializeField] protected CardElement element; // 속성 (추후 확장용)
}
