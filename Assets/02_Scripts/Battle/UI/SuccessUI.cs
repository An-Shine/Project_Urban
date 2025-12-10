using UnityEngine;

public class SuccessUI : MonoBehaviour
{
    [SerializeField] private RewardUI resultUI;
    [SerializeField] private CardSelectUI cardSelectUI;

    private void Awake()
    {
        ShowResult();
    }

    public void ShowResult()
    {
        resultUI.gameObject.SetActive(true);
        cardSelectUI.gameObject.SetActive(false);
    }    

    public void ShowCardSelect()
    {
        resultUI.gameObject.SetActive(false);
        cardSelectUI.gameObject.SetActive(true);
    }
}