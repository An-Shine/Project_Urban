using UnityEngine;

[System.Serializable]
public class AttackCard : Card
{    
    public int damage;          // 데미지
    public int cost;            // 코스트
    public CardElement element; // 속성

   
    public AttackCard(int _damage, int _cost, CardElement _element)
    {
        this.damage = _damage;
        this.cost = _cost;
        this.element = _element;
    }

    //타겟 확인
    public bool IsTargetValid(GameObject target)
    {
        // 1. 대상이 존재하는지 확인
        if (target == null) return false;

        // 2. 대상의 태그 확인         .
        if (target.CompareTag("Player") || target.CompareTag("Enemy"))
        {
            return true;
        }        
        return false;
    }
}