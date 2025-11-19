using UnityEngine;
using UnityEngine.Events;

abstract public class PointController : MonoBehaviour
{
    [SerializeField] protected int maxPoint;
    [SerializeField] protected int minPoint;
    [SerializeField] protected int curPoint;

    public int CurrentPoint => maxPoint;
    public UnityEvent<int> OnUpdatePoint { get; set; } = new();
    public UnityEvent<int> OnUpdateMaxPoint { get; set; } = new();

    public void Increase(int amount = 1)
    {
        curPoint += amount;
        
        if (curPoint > maxPoint)
            curPoint = maxPoint;

        OnUpdatePoint?.Invoke(curPoint);
    }

    public void Decrease(int amount = 1)
    {
        curPoint -= amount;

        if (curPoint < minPoint)
            curPoint = minPoint;

        OnUpdatePoint?.Invoke(curPoint);
    }

    public void ExpandMax(int amount)
    {
        maxPoint += amount;
        OnUpdateMaxPoint?.Invoke(maxPoint);
    }

    public void ReduceMax(int amount)
    {
        maxPoint -= amount;

        if (maxPoint < minPoint)
            maxPoint = minPoint;

        OnUpdateMaxPoint?.Invoke(maxPoint);
    }

    public void Reset()
    {
        ResetPoint();
        OnUpdatePoint?.Invoke(curPoint);
    }

    abstract protected void ResetPoint();
}