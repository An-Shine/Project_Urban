using UnityEngine;
using UnityEngine.Events;

abstract public class PointController : MonoBehaviour
{
    [SerializeField] protected int maxPoint;
    [SerializeField] protected int minPoint;
    [SerializeField] protected int curPoint;

    public int CurrentPoint => maxPoint;
    public UnityEvent OnUpdatePoint { get; set; }

    public void Increase(int amount = 1)
    {
        curPoint += amount;
        
        if (curPoint > maxPoint)
            curPoint = maxPoint;
    }

    public void Decrease(int amount = 1)
    {
        curPoint -= amount;

        if (curPoint < minPoint)
            curPoint = minPoint;
    }

    public void ExpandMax(int amount)
    {
        maxPoint += amount;
    }

    public void ReduceMax(int amount)
    {
        maxPoint -= amount;

        if (maxPoint < minPoint)
            maxPoint = minPoint;
    }

    abstract public void Reset();
}