using UnityEngine;

public class SelectElement : MonoBehaviour
{
    [SerializeField] private GameObject selectPanel;

    [SerializeField] private Player player;

    private Deck deck;

    private void Start()
    {
        // 1. 플레이어 찾기 
        if (player == null)
        {
            player = FindFirstObjectByType<Player>();
        }

        // 2. 덱 찾기 
        if (player != null)
        {
            deck = player.GetComponent<Deck>();
        }

        if (deck == null) Debug.LogError("SelectElement: Deck을 찾을 수 없습니다!");
    }

    public void SelectFlame()
    {
        Debug.Log("불꽃 속성 선택");
        AddCardsAndGameStart(CardName.Ignite, CardName.FlameBarrier);
    }

    public void SelectIce()
    {
        Debug.Log("얼음 속성 선택");
        AddCardsAndGameStart(CardName.GlacialWedge, CardName.IceShield);
    }

    public void SelectGrass()
    {
        Debug.Log("풀 속성 선택");
        AddCardsAndGameStart(CardName.DoubleEdgedSword, CardName.ElasticBarrier);
    }

   private void AddCardsAndGameStart(CardName card1, CardName card2)
    {
        if (deck == null) return;

        // 1. 카드 2장 추가해둡니다. (원본 리스트에 추가)
        deck.AddCard(card1);
        deck.AddCard(card2);    

        Debug.Log($"속성 카드 2장 추가 완료! 현재 덱은 총 {deck.OriginCardCount}장입니다.");   
        
        // 2. UI 끄기
        if (selectPanel != null) selectPanel.SetActive(false);

        // 3. 게임 시작
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.StartBattle();
        }
    }
}