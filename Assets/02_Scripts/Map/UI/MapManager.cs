using UnityEngine;

public class MapManager : SceneSingleton<MapManager>
{
    public static MapManager Instance; 

    [Header("맵 상태 UI 연결")]
    //[SerializeField] private GameObject selectElementUI; 
    [SerializeField] private GameObject stageSelectUI;   

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    { 
        bool isReturning = GameManager.Instance.IsStageSelectMode;    

        if (isReturning)
        {            
            //selectElementUI.SetActive(false);
            stageSelectUI.SetActive(true);
            GameManager.Instance.IsStageSelectMode = false;
        }
        else
        {
            //selectElementUI.SetActive(true);
            stageSelectUI.SetActive(false);
        }        
    }  
}