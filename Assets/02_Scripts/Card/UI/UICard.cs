using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class UICard : MonoBehaviour
{
    [SerializeField] private TMP_Text cardTitle;
    [SerializeField] private TMP_Text cardDesc;
    [SerializeField] private Image cardImage;       

    private CardDataEntry currentData;
    
    public void SetCardDataEntry(CardDataEntry data, UnityAction<CardDataEntry> onClickCallback = null)
    {
        currentData = data;
        
        cardImage.sprite = data.cardSprite;
        cardTitle.text = data.koreanName;
        cardDesc.text = data.description;           
        
    }
   
}