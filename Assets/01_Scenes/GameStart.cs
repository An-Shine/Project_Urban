using UnityEngine;
using UnityEngine.SceneManagement;


public class GameStart : MonoBehaviour
{
    public void OnGameStart()
    {
        SceneManager.LoadScene("02_ElementSelectScene");
    }
}
