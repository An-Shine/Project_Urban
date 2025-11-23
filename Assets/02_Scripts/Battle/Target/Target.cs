using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HpController))]
[RequireComponent(typeof(ShieldController))]
abstract public class Target : MonoBehaviour
{
    private HpController hpController;
    private ShieldController shieldController;

    protected virtual void Awake()
    {
        hpController = GetComponent<HpController>();
        shieldController = GetComponent<ShieldController>();

        // TODO : 나중에 GameManager에서 받아서 HP 설정하기
        hpController.ResetPoint();
        
        shieldController.ResetPoint();
    }

    public UnityEvent OnDie = new();
    public UnityEvent<bool> OnDamaged = new();

    public UnityEvent<int> OnUpdateHp => hpController.OnUpdatePoint;
    public UnityEvent<int> OnUpdateMaxHp => hpController.OnUpdateMaxPoint;
    public UnityEvent<int> OnUpdateSheild => shieldController.OnUpdatePoint;


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

    public virtual void Reset()
    {
        shieldController.Reset();
    }
}
