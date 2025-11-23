using UnityEngine;

abstract public class DebuffCard : Card
{
    //디버프카드 데이터
    [SerializeField] protected int debuffAmount;    // 디버프 수치
    [SerializeField] protected int duration;        // 지속 턴 수
    [SerializeField] protected CardElement element; // 속성
}