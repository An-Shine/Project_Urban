using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동을 위해 필수

public class SelectManager : MonoBehaviour
{
    private Dictionary<Element, CardName[]> elementCardMaps = new();

    // 1. Flame 속성 선택 버튼 연결
    public void OnClickFlame()
    {   
        GameManager.Instance.SelectedElement = Element.Flame;
        GameManager.Instance.SetBonusCards(CardName.Ignite, CardName.FlameBarrier);
        
        GoToBattleScene();
    }

    // 2. Ice 속성 선택 버튼 연결
    public void OnClickIce()
    {
        GameManager.Instance.SelectedElement = Element.Ice;
        GameManager.Instance.SetBonusCards(CardName.GlacialWedge, CardName.IceShield);
        GoToBattleScene();
    }

    // 3. Grass 속성 선택 버튼 연결
    public void OnClickGrass()
    {
        GameManager.Instance.SelectedElement = Element.Grass;
        GameManager.Instance.SetBonusCards(CardName.DoubleEdgedSword, CardName.ElasticBarrier);
        GoToBattleScene();
    }

    private void GoToBattleScene()
    {        
        SceneManager.LoadScene("Battle_Scene");
    }
}