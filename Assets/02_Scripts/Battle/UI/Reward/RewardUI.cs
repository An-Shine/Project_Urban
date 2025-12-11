using TMPro;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    private void Awake()
    {
        coinText.text = $"{BattleManager.Instance.EarnedCoin} Coin";
    }
}