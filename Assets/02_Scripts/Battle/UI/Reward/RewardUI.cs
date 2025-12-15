using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Button cardSelectButton;
    private bool isCardSelected;
    

    private void Awake()
    {
        coinText.text = $"{BattleManager.Instance.EarnedCoin} Coin";
    }
}