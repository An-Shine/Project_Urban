using UnityEngine;

public class OpenRewardUI : MonoBehaviour
{
    [SerializeField] private GameObject stageClearUI; // 비활성화되어 있는 StageClearUI 오브젝트

   
    public void OnClickOpen()
    {
        if (stageClearUI != null)
        {
            stageClearUI.SetActive(true);
        } 
       
    }
}