using UnityEngine;
using UnityEngine.SceneManagement;

public class SuccessUI : MonoBehaviour
{
    [SerializeField] private RewardUI rewardUI;
    [SerializeField] private CardSelectUI cardSelectUI;

    private void Awake()
    {
        ShowReward();
    }

    public void ShowReward()
    {
        rewardUI.gameObject.SetActive(true);
        cardSelectUI.gameObject.SetActive(false);
    }    

    public void ShowCardSelect()
    {
        rewardUI.gameObject.SetActive(false);
        cardSelectUI.gameObject.SetActive(true);
    }

    public void LoadMapScene()
    {
        SceneManager.LoadScene("Map_Scene");
    }
}