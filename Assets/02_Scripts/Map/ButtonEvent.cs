using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; 


public class ButtonEvent : MonoBehaviour
{

    private GameObject UI_Store;
    private GameObject UI_Shelter;
    private GameObject stageSelectButton;
    // 전투 씬 이동
    public void OnClickNormal()
    {
        SceneManager.LoadScene(Scene.Battle);
    }

    public void OnClickBoss()
    {
        SceneManager.LoadScene(Scene.Battle);
    }

    // 상점UI
    public void OnClickStore()
    {        
        UI_Store.SetActive(true);
        //stageSelectButton.SetActive(false);
    }

    public void OnClickStoreExit()
    {
        UI_Store.SetActive(false);
        //stageSelectButton.SetActive(true);
    }

    // 쉼터UI
    public void OnClickShelter()
    {
       UI_Shelter.SetActive(true);
       //stageSelectButton.SetActive(false);
    }
}