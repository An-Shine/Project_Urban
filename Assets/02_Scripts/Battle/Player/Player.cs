using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HpController))]
[RequireComponent(typeof(CostController))]
[RequireComponent(typeof(ShieldController))]
public class Player : MonoBehaviour
{
    // Component
    private HpController hpController;
    private CostController costController;
    private ShieldController shieldController;

    // Event
    public UnityEvent OnDie = new();
    public UnityEvent<bool> OnDamaged = new();
    public UnityEvent<int> OnUpdateHp => hpController.OnUpdatePoint;
    public UnityEvent<int> OnUpdateMaxHp => hpController.OnUpdateMaxPoint;
    public UnityEvent<int> OnUpdateCost => costController.OnUpdatePoint;
    public UnityEvent<int> OnUdpateMaxCost => costController.OnUpdateMaxPoint;
    public UnityEvent<int> OnUpdateSheild => shieldController.OnUpdatePoint;


    private void Awake()
    {
        hpController = GetComponent<HpController>();
        costController = GetComponent<CostController>();
        shieldController = GetComponent<ShieldController>();
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
    }

    public void Reset()
    {
        costController.Reset();
        shieldController.Reset();
    }
}
