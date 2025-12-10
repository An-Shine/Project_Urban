using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CardSelectItem : MonoBehaviour
{
    public void SetCardInfo(CardName cardName)
    {
        Image image = GetComponent<Image>();
    }
}