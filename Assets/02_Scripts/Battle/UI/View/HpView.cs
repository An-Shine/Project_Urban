using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpView : MonoBehaviour, IPointView
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;

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
        hpText.text = $"{curHp}/{maxHp}";
    }

    public void Bind(PointController controller)
    {
        controller.OnUpdate.AddListener(UpdateView);
        UpdateView(controller.CurrentPoint, controller.MaxPoint);
    }
}