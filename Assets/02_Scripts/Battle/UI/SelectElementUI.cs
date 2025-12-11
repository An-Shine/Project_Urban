using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SelectManager : MonoBehaviour
{
    [SerializeField] private List<CardName> cards = new();
    private readonly Dictionary<Element, List<CardName>> elementCardMaps = new();

    private void Awake()
    {
        foreach (CardName name in cards)
        {
            Element element = Util.GetElement(name);

            if (!elementCardMaps.ContainsKey(element))
                elementCardMaps[element] = new();

            elementCardMaps[element].Add(name);
        }
    }

    public void SelectElement(Element element)
    {
        GameManager.Instance.SelectedElement = element;
        GameManager.Instance.SetBonusCards(elementCardMaps[element]);

        SceneManager.LoadScene("Battle_Scene");
    }

    public void OnClickFlame()
    {
        SelectElement(Element.Flame);
    }

    // 2. Ice 속성 선택 버튼 연결
    public void OnClickIce()
    {
       SelectElement(Element.Ice);
    }

    // 3. Grass 속성 선택 버튼 연결
    public void OnClickGrass()
    {
       SelectElement(Element.Grass);
    }
}