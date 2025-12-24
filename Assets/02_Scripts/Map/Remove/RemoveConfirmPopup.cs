using UnityEngine;
using TMPro;
using Michsky.UI.Dark;

[RequireComponent(typeof(ModalWindowManager))]
public class RemoveConfirmPopup : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UICard targetCardUI; 

    private CardDataEntry targetCard;    
    private ModalWindowManager mwManager;

    private void Awake()
    {
        mwManager = GetComponent<ModalWindowManager>();
    }

    public void OpenPopup(CardDataEntry card)
    {
        targetCard = card;
        targetCardUI.SetCardDataEntry(card);
        mwManager.ModalWindowIn();
    }

    public void OnClickRemove()
    {  
        GameManager.Instance.Deck.RemoveCard(targetCard.cardName);
        ClosePopup();
    }

    public void OnClickCancel()
    {
        ClosePopup();
    }

    private void ClosePopup()
    {
        mwManager.ModalWindowOut();
    }
}