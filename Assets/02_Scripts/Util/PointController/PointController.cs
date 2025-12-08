using System;
using UnityEngine.Events;

public static class PointControllerFactory
{
    public static HpController CreateHp(int max) => new(max, max);
    public static ProtectController CreateProtect(int max) => new(max, 0);
    public static CostController CreateCost(int max) => new(max, max);
    public static TurnController CreateTurn(int max) => new(max, max);
}

public class PointController
{
    private int maxPoint;
    private int initPoint;
    private int curPoint;

    public PointController(int maxPoint, int initPoint)
    {
        this.maxPoint = maxPoint;
        this.initPoint = initPoint;
        this.curPoint = initPoint;
    }

    public void Reset()
    {
        curPoint = initPoint;
    }

    public int MaxPoint
    {
        get { return maxPoint;}
        set
        {
            if (value < 0)
                throw new ArgumentException();

            maxPoint = value;
        }
    }

    public int CurrentPoint
    {
        get { return curPoint; }
        set
        {
            if (value < 0 || value > maxPoint)
                throw new ArgumentException();

            curPoint = value;
        }
    }

    public UnityEvent<int, int> OnUpdate { get; } = new();

    public void Increase(int amount = 1)
    {
        curPoint += amount;

        if (curPoint > maxPoint)
            curPoint = maxPoint;

        OnUpdate?.Invoke(curPoint, maxPoint);
    }

    public void Increase(float per)
    {
        Increase((int)(maxPoint * per));
    }

    public void Decrease(int amount = 1)
    {
        curPoint -= amount;

        if (curPoint < 0)
            curPoint = 0;

        OnUpdate?.Invoke(curPoint, maxPoint);
    }

    public void Decrease(float per)
    {
        Decrease((int)(maxPoint * per));
    }

    public void ExpandMax(int amount)
    {
        maxPoint += amount;
        OnUpdate?.Invoke(curPoint, maxPoint);
    }

    public void ExpandMax(float per)
    {
        ExpandMax((int)(maxPoint * per));
    }

    public void ReduceMax(int amount)
    {
        maxPoint -= amount;

        if (maxPoint < 0)
            maxPoint = 0;

        OnUpdate?.Invoke(curPoint, maxPoint);
    }

    public void ReduceMax(float per)
    {
        ExpandMax((int)(maxPoint * per));
    }
}