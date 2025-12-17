using TMPro;
using UnityEngine;
public class CostView : MonoBehaviour, IPointView
{
    [SerializeField] TMP_Text costText;

    public void UpdateView(int curCost, int maxCost)
    {
        costText.text = $"{curCost}/{maxCost}";
    }

    public void Bind(PointController controller)
    {
        controller.OnUpdate.AddListener(UpdateView);
        UpdateView(controller.CurrentPoint, controller.MaxPoint);
    }
}   
