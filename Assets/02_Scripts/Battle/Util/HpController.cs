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
                hpBar.HPBar(current, max);
            }
        });
        Reset();
    }

    public override void ResetPoint()
    {
        curPoint = maxPoint;
    }
    
}
