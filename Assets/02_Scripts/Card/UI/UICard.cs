using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class UICard : MonoBehaviour
{
    [SerializeField] private TMP_Text cardTitle;
    [SerializeField] private TMP_Text cardDesc;
    [SerializeField] private Image cardImage;       
    
    public void SetCardDataEntry(CardDataEntry data)
    {
        cardImage.sprite = data.cardSprite;
        cardTitle.text = data.koreanName;
        cardDesc.text = data.description;           
    }

    public void AddClickEventHandler(UnityAction clickHandler)
    {
        GetComponent<Button>().onClick.AddListener(clickHandler);
    }
}