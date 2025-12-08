using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

abstract public class Target : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected int maxHp;
    [SerializeField] protected int maxProtect;
    [SerializeField] protected Element element;

    [Header("Base View Settings")]
    [SerializeField] protected HpView hpView;
    [SerializeField] protected ProtectView protectView;
    [SerializeField] protected ConditionStatusView conditionStatusView;

    // 버프-디버프 관리
    protected List<ConditionStatus> conditionStatusList = new();

    // Property
    public HpController Hp { get; protected set; }
    public ProtectController Protect { get; protected set; }

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
        int shieldPoints = Protect.CurrentPoint;
        int damageToShield = Mathf.Min(shieldPoints, hitPoint);
        int remainingDamage = hitPoint - damageToShield;

        // 방패에 데미지 적용
        if (damageToShield > 0)
            Protect.Decrease(damageToShield);

        // 남은 데미지를 체력에 적용
        if (remainingDamage > 0)
            Hp.Decrease(remainingDamage);

        if (Hp.CurrentPoint <= 0)
            OnDead?.Invoke(this);
        else
            OnDamaged?.Invoke(this, Protect.CurrentPoint > 0);

        Debug.Log($"{gameObject.name} : {Hp.CurrentPoint}, hitPoint : {hitPoint}");
    }

    public void AddProtect(int protectPoint)
    {
        Protect.Increase(protectPoint);
    }

    public void AddConditionStatus(ConditionStatus conditionStatus)
    {
        conditionStatusList.Add(conditionStatus);
        conditionStatusView?.UpdateView(conditionStatusList);
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
