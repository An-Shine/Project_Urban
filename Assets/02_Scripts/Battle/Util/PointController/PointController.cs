using UnityEngine;
using UnityEngine.Events;

abstract public class PointController : MonoBehaviour
{
    [SerializeField] protected int maxPoint;
    [SerializeField] protected int minPoint;   

    protected int curPoint;

    public int MaxPoint => maxPoint;
    public int MinPoint => minPoint;
    public int CurrentPoint => curPoint;

    public UnityEvent<int> OnUpdatePoint { get; } = new();
    public UnityEvent<int> OnUpdateMaxPoint { get; } = new();

    public void Increase(int amount = 1)
    {
        curPoint += amount;
        
        if (curPoint > maxPoint)
            curPoint = maxPoint;

        OnUpdatePoint?.Invoke(curPoint);
    }

    public void Increase(float per)
    {
        Increase((int)(maxPoint * per));
    }

    public void Decrease(int amount = 1)
    {
        curPoint -= amount;

        if (curPoint < minPoint)
            curPoint = minPoint;

        OnUpdatePoint?.Invoke(curPoint);
    }

    public void Decrease(float per)
    {
        Decrease((int)(maxPoint * per));
    }

    public void ExpandMax(int amount)
    {
        maxPoint += amount;
        OnUpdateMaxPoint?.Invoke(maxPoint);
    }

    public void ExpandMax(float per)
    {
        ExpandMax((int)(maxPoint * per));
    }

    public void ReduceMax(int amount)
    {
        maxPoint -= amount;

        if (maxPoint < minPoint)
            maxPoint = minPoint;

        OnUpdateMaxPoint?.Invoke(maxPoint);
    }

    public void ReduceMax(float per)
    {
        ExpandMax((int)(maxPoint * per));
    }

    public void Reset()
    {
        ResetPoint();
        OnUpdatePoint?.Invoke(curPoint);
    }

    abstract protected void ResetPoint();
}