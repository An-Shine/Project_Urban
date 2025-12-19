using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ConditionStatusView : MonoBehaviour
{
    [SerializeField] private TMP_Text costText;
    private readonly StringBuilder stringBuilder = new();

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
