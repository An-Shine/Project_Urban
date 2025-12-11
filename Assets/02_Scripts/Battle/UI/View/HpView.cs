using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HpView : MonoBehaviour, IPointView
{
    private Slider hpSlider;

    private void Awake()
    {
        hpSlider = GetComponent<Slider>();
    }

    // 0나누기 방지 
    public void UpdateView(int curHp, int maxHp)
    {
        if(maxHp == 0)
        {
            hpSlider.value = 0;
            return;
        }

        float ratio = (float)curHp / maxHp;
        hpSlider.value = ratio;
    }

    public void Bind(PointController controller)
    {
        controller.OnUpdate.AddListener(UpdateView);
        UpdateView(controller.CurrentPoint, controller.MaxPoint);
    }
}