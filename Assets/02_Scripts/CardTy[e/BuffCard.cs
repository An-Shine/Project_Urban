using UnityEngine;

[System.Serializable]
public class BuffCard : Card
{
    //버프카드 
    public int buffAmount;      // 버프 수치
    public int duration;        // 지속 턴 수
    public int cost;            // 코스트
    public CardElement element; // 속성

    //버프 수치설정
    public BuffCard(int _buffAmount, int _duration, int _cost, CardElement _element)
    {
        this.buffAmount = _buffAmount;
        this.duration = _duration;
        this.cost = _cost;
        this.element = _element;
    }

    //타겟확인메커니즘    
    public bool IsTargetValid(GameObject target)
    {
        // 대상 존재 여부
        if (target == null) return false;

        // 태그 확인 (Player 또는 Enemy)        
        if (target.CompareTag("Player") || target.CompareTag("Enemy"))
        {
            return true;
        }

        return false;
    }
}