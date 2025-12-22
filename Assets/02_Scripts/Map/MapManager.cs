using UnityEngine;

public class MapManager : SceneSingleton<MapManager>
{
    [Header("맵 상태 UI 연결")]
    [SerializeField] private GameObject stageSelectUI;   

    private void Start()
    { 
        stageSelectUI.SetActive(true);
    }  
}