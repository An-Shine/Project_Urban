using UnityEngine;

abstract public class BuffCard : Card
{
    //버프카드 
    [SerializeField] protected int buffAmount;      // 버프 수치
    [SerializeField] protected int duration;        // 지속 턴 수
    [SerializeField] protected CardElement element; // 속성
}