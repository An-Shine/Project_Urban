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
        // 대상 존재 여부
        if (target == null) return false;

        // 태그 확인
        if (target.CompareTag("Player") || target.CompareTag("Enemy"))
        {
            return true;
        }
        
        return false;
    }
}
