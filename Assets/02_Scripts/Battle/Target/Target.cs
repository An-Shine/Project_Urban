using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HpController))]
[RequireComponent(typeof(ShieldController))]
abstract public class Target : MonoBehaviour
{
    protected HpController hpController;
    protected ShieldController shieldController;

    public HpController HpController => hpController;
    public ShieldController ShieldController => shieldController;

    public UnityEvent OnDie = new();
    public UnityEvent<bool> OnDamaged = new();

    private void Awake()
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

        Debug.Log(hpController.CurrentPoint);
    }

    public void AddShield(int shieldPoint)
    {
        shieldController.Increase(shieldPoint);
    }

    protected virtual void Init()
    {
        hpController = GetComponent<HpController>();
        shieldController = GetComponent<ShieldController>();
    }

    protected virtual void Reset()
    {
        hpController.Reset();
        shieldController.Reset();
    }
}
