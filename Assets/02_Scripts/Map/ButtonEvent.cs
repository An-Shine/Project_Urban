using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; 


public class ButtonEvent : SceneSingleton<ButtonEvent>
{

    [SerializeField] private GameObject UI_Store;
    [SerializeField] private GameObject UI_Shelter;
    [SerializeField] private GameObject UI_Map;
    // 전투 씬 이동
    public void OnClickNormal()
    {
        SceneManager.LoadScene(Scene.Battle);
    }

    public void OnClickElite()
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
        UI_Map.SetActive(false);
    }

    public void OnClickStoreExit()
    {
        UI_Store.SetActive(false);
        UI_Map.SetActive(true);
    }

    // 쉼터UI
    public void OnClickShelter()
    {
       UI_Shelter.SetActive(true);
       UI_Map.SetActive(false);
    }

    public void ActiveMap()
    {
        UI_Map.SetActive(true);
    }

    public void OnClickEvent()
    {
        Debug.Log("이벤트 발생!");
    }
}