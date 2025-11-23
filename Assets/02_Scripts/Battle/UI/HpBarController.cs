using System;
using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    [SerializeField] private HpController hpController;
    [SerializeField] private Slider hpSlider;

    // 0나누기 방지 
    public void HPBar(float current, float max)
    {
        if(max == 0)
        {
            hpSlider.value = 0;
            return;
        }

        float ratio = current / max;

        hpSlider.value = ratio;
    }
    
}