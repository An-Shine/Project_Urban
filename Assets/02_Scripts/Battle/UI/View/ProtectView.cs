using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProtectView : MonoBehaviour, IPointView
{
    [SerializeField] private Slider protectSlider;
    [SerializeField] private TMP_Text protectText;

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
        protectText.text = $"{curProtect}/{maxProtect}";
    }

    public void Bind(PointController controller)
    {
        controller.OnUpdate.AddListener(UpdateView);
        UpdateView(controller.CurrentPoint, controller.MaxPoint);
    }
}