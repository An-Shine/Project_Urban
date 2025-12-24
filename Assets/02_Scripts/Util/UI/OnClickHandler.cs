using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OnClickHandler : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void AddClickHandler(UnityAction handleClick)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(handleClick);
    }
} 