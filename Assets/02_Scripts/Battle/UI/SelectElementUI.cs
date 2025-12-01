using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동을 위해 필수

public class SelectManager : MonoBehaviour
{
    // 1. Flame 속성 선택 버튼 연결
    public void OnClickFlame()
    {        
        GameManager.Instance.SetBonusCards(CardName.Ignite, CardName.FlameBarrier);
        
        GoToBattleScene();
    }

    // 2. Ice 속성 선택 버튼 연결
    public void OnClickIce()
    {
        GameManager.Instance.SetBonusCards(CardName.GlacialWedge, CardName.IceShield);
        GoToBattleScene();
    }

    // 3. Grass 속성 선택 버튼 연결
    public void OnClickGrass()
    {
        GameManager.Instance.SetBonusCards(CardName.DoubleEdgedSword, CardName.ElasticBarrier);
        GoToBattleScene();
    }

    private void GoToBattleScene()
    {        
        SceneManager.LoadScene("Battle_Scene");
    }
}