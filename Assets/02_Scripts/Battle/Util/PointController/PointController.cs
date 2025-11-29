using System;
using UnityEngine;
using UnityEngine.Events;

abstract public class PointController : MonoBehaviour
{
    [SerializeField] protected int maxPoint;
    [SerializeField] protected int minPoint;

    protected int curPoint;

    public int MaxPoint
    {
        get { return maxPoint;}
        set
        {
            if (value < 0 || value < minPoint)
                throw new ArgumentException();

            maxPoint = value;
        }
    }

    public int MinPoint
    {
        get { return minPoint;}
        set
        {
            if (value < 0 || value > maxPoint)
                throw new ArgumentException();

            minPoint = value;
        }
    }

    public int CurrentPoint
    {
        get { return curPoint; }
        set
        {
            if (value < 0 || value > maxPoint || value < minPoint)
                throw new ArgumentException();

            curPoint = value;
        }
    }

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