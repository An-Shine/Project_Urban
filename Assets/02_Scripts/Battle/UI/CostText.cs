using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CostText : MonoBehaviour
{
    private Text costText;
    private int curCost;
    private int maxCost;
    
    public void Init(CostController costController)
    {
        costText = GetComponent<Text>();
        curCost = costController.CurrentPoint;
        maxCost = costController.MaxPoint;

        costController.OnUpdatePoint.AddListener(curCost => SetCurCost(curCost));
        costController.OnUpdateMaxPoint.AddListener(maxCost => SetMaxCost(maxCost));
    
        UpdateText();
    }

    public void SetCurCost(int curCost)
    {
        this.curCost = curCost;
        UpdateText();
    }

    public void SetMaxCost(int maxCost)
    {
        this.maxCost = maxCost;
        UpdateText();
    }

    private void UpdateText()
    {
        costText.text = $"{curCost} / {maxCost}";
    }
}   
