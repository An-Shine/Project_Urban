using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ConditionStatusView : MonoBehaviour
{
    private TMP_Text costText;
    private readonly StringBuilder stringBuilder = new();

    private void Awake()
    {
        costText = GetComponent<TMP_Text>();
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
