using System;
using UnityEngine;

public class HpController : PointController
{

    [SerializeField] private HpBarController hpBar;

    void Start()
    {
        OnUpdatePoint.AddListener((current, max) =>
        {
            if(hpBar != null)
            {
                hpBar.SetHp(current, max);
            }
        });
        Reset();
    }

    public override void ResetPoint()
    {
        curPoint = maxPoint;
    }
    
}
