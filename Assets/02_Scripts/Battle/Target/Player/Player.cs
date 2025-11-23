using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ShieldController))]
public class Player : Target
{
    // Component
    private CostController costController;

    // Event
    public UnityEvent<int> OnUpdateCost => costController.OnUpdatePoint;
    public UnityEvent<int> OnUdpateMaxCost => costController.OnUpdateMaxPoint;

    protected override void Awake()
    {
        base.Awake();   
        costController = GetComponent<CostController>();
    }

    public override void Reset()
    {
        base.Reset();
        costController.Reset();
    }
}
