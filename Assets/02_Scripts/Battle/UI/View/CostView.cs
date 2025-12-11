using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CostView : MonoBehaviour, IPointView
{
    private Text costText;

    private void Awake()
    {
        costText = GetComponent<Text>();
    }

    public void UpdateView(int curCost, int maxCost)
    {
        costText.text = $"{curCost} / {maxCost}";
    }

    public void Bind(PointController controller)
    {
        controller.OnUpdate.AddListener(UpdateView);
        UpdateView(controller.CurrentPoint, controller.MaxPoint);
    }
}   
