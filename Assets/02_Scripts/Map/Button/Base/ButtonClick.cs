using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
abstract public class NodeButton : MonoBehaviour
{
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void AddOnClickEvent(UnityAction action)
    {
 
        button.onClick.AddListener(action);
    }

    abstract public void OnClick();
}
