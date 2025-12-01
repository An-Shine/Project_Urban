using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(HpController))]
[RequireComponent(typeof(ShieldController))]
abstract public class Target : MonoBehaviour
{
    [SerializeField] protected Element element;

    // Controller
    protected HpController hpController;
    protected ShieldController shieldController;
    protected List<ConditionStatus> conditionStatusList = new();
    
    // Property
    public HpController HpController => hpController;
    public ShieldController ShieldController => shieldController;

    // Event
    public UnityEvent OnDie = new();
    public UnityEvent<bool> OnDamaged = new();

    // Status    
    protected bool isStun = false;

    protected virtual void Start()
    {
        Init();
        Reset();
    }

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

        if (conditionStatus is Stun)
            isStun = true;
    }

    protected virtual void Init()
    {
        hpController = GetComponent<HpController>();
        shieldController = GetComponent<ShieldController>();

        BattleManager.Instance.OnTurnEnd.AddListener(TurnEndHandler);
    }

    private void TurnEndHandler()
    {
        foreach (var conditionStatus in conditionStatusList)
        {
            conditionStatus.Execute(this);
        }

        conditionStatusList.RemoveAll(status => status.RemainingTurn <= 0);

        isStun = false;
        foreach (var conditionStatus in conditionStatusList)
        {
            if (conditionStatus is Stun)
            {
                isStun = true;    
                break;
            }
        }
    }

    public virtual void Reset()
    {
        hpController.Reset();
        shieldController.Reset();
        conditionStatusList.Clear();
    }
}
