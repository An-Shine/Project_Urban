using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동을 위해 필수!

public class MapManager : MonoBehaviour
{

    public enum StageType
    {
        Normal,     //기본 몬스터 스테이지
        Shelter,    //휴식 스테이지
        Store,      //상점 스테이지
        Boss        //보스 스테이지
    }

    [Header("UI 패널 연결")]
    [SerializeField] private GameObject shelterUI; // 휴식 UI
    [SerializeField] private GameObject Store_UI;   // 상점 UI
    [SerializeField] private GameObject selectElementUI; // 속성 선택 UI
    [SerializeField] private GameObject stageSelectUI;

    private void Start()
    {
        bool isReturning = GameManager.Instance.IsStageSelectMode;    

        if (isReturning)
        {            
            if (selectElementUI != null) selectElementUI.SetActive(false);
            if (stageSelectUI != null) stageSelectUI.SetActive(true);
           
            GameManager.Instance.IsStageSelectMode = false;
        }
        else
        {
            if (selectElementUI != null) selectElementUI.SetActive(true);
            if (stageSelectUI != null) stageSelectUI.SetActive(false);
        }
    }

    // 1. Normal 버튼 기능
    public void OnClickNormal()
    {
        SceneManager.LoadScene("Battle_Scene");
    }

    // 2. Shelter 버튼 기능
    public void OnClickShelter()
    {
        shelterUI.SetActive(true);
    }

    // 3. Store 버튼 기능
    public void OnClickStore()
    {
        Store_UI.SetActive(true);  
        stageSelectUI.SetActive(false);
    }

    // 4. Boss 버튼 기능
    public void OnClickBoss()
    {
        SceneManager.LoadScene("Battle_Scene");
    }
    

       
    
}