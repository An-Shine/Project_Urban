using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ButtonEvent : MonoBehaviour
{
    [Header("메인 UI 연결")]
    [SerializeField] private GameObject Store_UI;
    [SerializeField] private GameObject Shelter_UI;


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
        Store_UI.SetActive(true);
    }

    // 쉼터UI
    public void OnClickShelter()
    {
       Shelter_UI.SetActive(true);
    }
}