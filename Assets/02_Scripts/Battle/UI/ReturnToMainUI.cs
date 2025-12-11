using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMain : MonoBehaviour
{
    public void OnClickReturn()
    {        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IsStageSelectMode = true;
        }

        // 씬 이동
        SceneManager.LoadScene("Map_Scene_AKH");
    }
}