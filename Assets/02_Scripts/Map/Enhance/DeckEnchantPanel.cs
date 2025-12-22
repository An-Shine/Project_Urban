using UnityEngine;
using System.Collections.Generic;

public class DeckEnchantPanel : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject panelObject;
    [SerializeField] private Transform contentArea;
    [SerializeField] private GameObject cardSlotPrefab; 
    [SerializeField] private DeckEnchantPopup enchantPopup;

    public void OpenEnchantPanel()
    {
        panelObject.SetActive(true);
        RenderDeck();
    }

    public void CloseEnchantPanel()
    {
        panelObject.SetActive(false);
    }

    private void RenderDeck()
    {
        foreach (Transform child in contentArea) Destroy(child.gameObject);
        
        List<CardDataEntry> currentDeck = ProtoTypeDeck.Instance.GetCurrentDeck();

        foreach (CardDataEntry entry in currentDeck)
        {
            GameObject newObj = Instantiate(cardSlotPrefab, contentArea);
            newObj.transform.localScale = Vector3.one;

            UICardProcess process = newObj.GetComponent<UICardProcess>();

            // 초기화
            process.Initialize(entry, CardProcessType.Event, (data) => 
            {
                enchantPopup.OpenPopup(data);
            });
        }
    }
}