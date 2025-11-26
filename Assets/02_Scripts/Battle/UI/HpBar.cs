using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HpBar : MonoBehaviour
{
    private Slider hpSlider;
    private int curHp;
    private int maxHp;

    public void Init(HpController hpController)
    {
        hpSlider = GetComponent<Slider>();
        
        curHp = hpController.MaxPoint;
        maxHp = hpController.MaxPoint;

        hpController.OnUpdatePoint.AddListener(curHp => SetCurrentHp(curHp));
        hpController.OnUpdateMaxPoint.AddListener(maxHp => SetMaxtHp(maxHp));
    
        UpdateBar();
    }

    private void SetCurrentHp(int curHp)
    {
        this.curHp = curHp;
        UpdateBar();
    }

    private void SetMaxtHp(int maxHp)
    {
        this.maxHp = maxHp;
        UpdateBar();
    }

    // 0나누기 방지 
    public void UpdateBar()
    {
        if(maxHp == 0)
        {
            hpSlider.value = 0;
            return;
        }

        float ratio = (float)curHp / maxHp;
        hpSlider.value = ratio;
    }
    
}