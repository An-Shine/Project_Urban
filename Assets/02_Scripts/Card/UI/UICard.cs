using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UICard : MonoBehaviour
{
    [SerializeField] private TMP_Text cardTitle;
    [SerializeField] private TMP_Text cardDesc;

    public void SetCardDataEntry(CardDataEntry cardDataEntry)
    {
        Image image = GetComponent<Image>();

        image.sprite = cardDataEntry.cardSprite;
        cardTitle.text = cardDataEntry.koreanName;
        cardDesc.text = cardDataEntry.description;
    }
}