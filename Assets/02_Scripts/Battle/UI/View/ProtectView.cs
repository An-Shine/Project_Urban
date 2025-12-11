using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ProtectView : MonoBehaviour, IPointView
{
    private Slider protectSlider;

    private void Awake()
    {
        protectSlider = GetComponent<Slider>();
    }

    // 0나누기 방지 
    public void UpdateView(int curProtect, int maxProtect)
    {
        if(maxProtect == 0)
        {
            protectSlider.value = 0;
            return;
        }

        float ratio = (float)curProtect / maxProtect;
        protectSlider.value = ratio;
    }

    public void Bind(PointController controller)
    {
        controller.OnUpdate.AddListener(UpdateView);
        UpdateView(controller.CurrentPoint, controller.MaxPoint);
    }
}