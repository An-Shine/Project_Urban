using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

abstract public class Target : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected Element element;

    [Header("Base View Settings")]
    [SerializeField] protected HealthView healthView;
    [SerializeField] protected ConditionStatusView conditionStatusView;

    // 버프-디버프 관리
    protected List<ConditionStatus> conditionStatusList = new();

    public UnityEvent<IEnumerable<ConditionStatus>> OnUpdateConditionStatusList = new();

    // Property
    public HealthController Health { get; protected set; }

    public Element Element
    {
        get { return element; }
        set { element = value; }
    }

    // Event
    public UnityEvent<Target> OnDead { get; } = new();
    public UnityEvent<Target, bool> OnDamaged { get; } = new();


    public void Damage(int hitPoint)
    {
        // 방패로 데미지 흡수
        int shieldPoints = Health.Protect;
        int damageToShield = Mathf.Min(shieldPoints, hitPoint);
        int remainingDamage = hitPoint - damageToShield;

        // 방패에 데미지 적용
        if (damageToShield > 0)
            Health.DecreaseProtect(damageToShield);

        // 남은 데미지를 체력에 적용
        if (remainingDamage > 0)
            Health.DecreaseHp(remainingDamage);

        if (Health.CurrentHp <= 0)
            OnDead?.Invoke(this);
        else
            OnDamaged?.Invoke(this, Health.Protect > 0);

        Debug.Log($"{gameObject.name} : {Health.CurrentHp}, hitPoint : {hitPoint}");
    }

    public void AddProtect(int protectPoint)
    {
        Health.IncreaseProtect(protectPoint);
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
