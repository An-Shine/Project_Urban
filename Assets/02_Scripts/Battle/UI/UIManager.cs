using UnityEngine;

public class UIManager : SceneSingleton<UIManager>
{
    [SerializeField] private Canvas battleUI;
    [SerializeField] private Canvas battleEndUI;
    [SerializeField] private CanvasRenderer successUI;
    [SerializeField] private CanvasRenderer failUI;

    private void Start()
    {
        battleUI.gameObject.SetActive(true);
        battleEndUI.gameObject.SetActive(false);

        BattleManager.Instance.OnBattleEnd.AddListener(HandleBattleEnd);
    }

    public void HandleBattleEnd(bool isSuccess)
    {
        battleUI.gameObject.SetActive(false);
        battleEndUI.gameObject.SetActive(true);

        if (isSuccess)
            successUI.gameObject.SetActive(true);
        else
            failUI.gameObject.SetActive(true);
    }
}