using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(HpController))]
[RequireComponent(typeof(ShieldController))]
abstract public class Target : MonoBehaviour
{   
    [Header("Initial Settings")]
    [SerializeField] protected Element element;

    // Controller
    protected HpController hpController;
    protected ShieldController shieldController;
    protected List<ConditionStatus> conditionStatusList = new();
    
    // Property
    public HpController HpController => hpController;
    public ShieldController ShieldController => shieldController;

    // Event
    [Header("Event")]
    public UnityEvent OnDie = new();
    public UnityEvent<bool> OnDamaged = new();

    public void Damage(int hitPoint)
    {
        // 방패로 데미지 흡수
        int shieldPoints = shieldController.CurrentPoint;
        int damageToShield = Mathf.Min(shieldPoints, hitPoint);
        int remainingDamage = hitPoint - damageToShield;

        // 방패에 데미지 적용
        if (damageToShield > 0)
            shieldController.Decrease(damageToShield);

        // 남은 데미지를 체력에 적용
        if (remainingDamage > 0)
            hpController.Decrease(remainingDamage);

        if (hpController.CurrentPoint <= 0)
            OnDie?.Invoke();
        else
            OnDamaged?.Invoke(shieldController.CurrentPoint > 0);

        Debug.Log($"{gameObject.name} : {hpController.CurrentPoint}, hitPoint : {hitPoint}");
    }

    public void AddShield(int shieldPoint)
    {
        shieldController.Increase(shieldPoint);
    }

    public void AddConditionStatus(ConditionStatus conditionStatus)
    {   
        conditionStatusList.Add(conditionStatus);
    }

    public bool IsStun()
    {
        foreach (ConditionStatus status in conditionStatusList)
        {
            if (status is Stun)
                return true;
        }

        return false;
    }
}
