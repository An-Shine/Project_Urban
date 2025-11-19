using UnityEngine;

[System.Serializable]
public class DefenseCard : Card
{
    //DefenseCard 데이터
    public int armor;           // 방어도 (Damage 대신 사용)
    public int cost;            // 코스트
    public CardElement element; // 속성 (추후 확장용)
    
    public DefenseCard(int _armor, int _cost, CardElement _element)
    {
        this.armor = _armor;
        this.cost = _cost;
        this.element = _element;
    }
    
    //타겟 확인
    public bool IsTargetValid(GameObject target)
    {
        // 1. 대상 존재 여부
        if (target == null) return false;

        // 2. 태그 확인 (Player 또는 Enemy면 OK)
        if (target.CompareTag("Player") || target.CompareTag("Enemy"))
        {
            return true;
        }

        // 3. 그 외(벽, 바닥 등)는 불가
        return false;
    }
}
