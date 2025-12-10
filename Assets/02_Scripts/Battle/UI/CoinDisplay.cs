using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    private void Start()
    {
        var myText = GetComponent<TextMeshProUGUI>();
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetCoinText(myText);
        }
    }
}