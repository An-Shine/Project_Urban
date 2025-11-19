using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HpController))]
[RequireComponent(typeof(CostController))]
public class Player : MonoBehaviour
{
    // Component
    private HpController hpController;
    private CostController costController;

    // Event
    public UnityEvent OnDie = new();
    public UnityEvent OnDamaged = new();
    public UnityEvent<int> OnUpdateHp => hpController.OnUpdatePoint;
    public UnityEvent<int> OnUpdateMaxHp => hpController.OnUpdateMaxPoint;
    public UnityEvent<int> OnUpdateCost => costController.OnUpdatePoint;
    public UnityEvent<int> OnUdpateMaxCost => costController.OnUpdateMaxPoint;


    private void Awake()
    {
        hpController = GetComponent<HpController>();
        costController = GetComponent<CostController>();
    }

    public void Damage(int hitPoint)
    {
        hpController.Decrease(hitPoint);

        if (hpController.CurrentPoint <= 0)
            OnDie?.Invoke();
        else
            OnDamaged?.Invoke();
    }

    public void Reset()
    {
        hpController.Reset();
        costController.Reset();
    }
}
