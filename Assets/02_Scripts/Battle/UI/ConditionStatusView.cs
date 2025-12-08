using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ConditionStatusView : MonoBehaviour
{
    private Text costText;
    private readonly StringBuilder stringBuilder = new();

    private void Awake()
    {
        costText = GetComponent<Text>();
    }

    public void UpdateView(IEnumerable<ConditionStatus> list)
    {
        stringBuilder.Clear();
        
        bool isFirst = true;
        foreach (ConditionStatus status in list)
        {
            if (!isFirst)
                stringBuilder.Append(", ");
            
            stringBuilder.Append(status.GetType().Name);
            isFirst = false;
        }
        
        costText.text = stringBuilder.ToString();
    }
}   
