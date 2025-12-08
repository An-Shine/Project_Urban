using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardCardList : MonoBehaviour
{
    [SerializeField] private Transform cardSpawnPoint; // 카드 생성 위치
    [SerializeField] private Button selectButton;      // 카드 선택 버튼 

    private ClearRewardUI rewardManager;
    private CardName myCardName;
    private GameObject currentCardObject;

    // 초기화
    public void Setup(ClearRewardUI manager, Card cardPrefab)
    {
        rewardManager = manager;
        myCardName = cardPrefab.Name;

        // 1. 기존 카드 제거 + 새 카드 생성
        if (currentCardObject != null) Destroy(currentCardObject);
        
        // 2. 카드프리펩 생성
        currentCardObject = Instantiate(cardPrefab.gameObject, cardSpawnPoint);
        
        // UI에 맞게 위치/크기/레이어 조정
        currentCardObject.transform.localPosition = new Vector3(0, 0, -500f);
        currentCardObject.transform.localRotation = Quaternion.identity;
        currentCardObject.transform.localScale = new Vector3(200.0f, 350.0f, 1.0f); // 크기 조절
        ChangeLayersRecursively(currentCardObject, "UI"); // UI 레이어로 변경

        var sg = currentCardObject.GetComponent<UnityEngine.Rendering.SortingGroup>();
        if (sg != null) 
        {
            sg.sortingOrder = 3000; // 아주 높은 값
        }
        
        // 3. 버튼 연결
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnSelect);
    }

    private void OnSelect()
    {
        rewardManager.OnRewardSelected(myCardName);
    }

    // 레이어 변경 (카드프리펩을 제일 위로)
    private void ChangeLayersRecursively(GameObject target, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer == -1) return;
        target.layer = layer;
        foreach (Transform child in target.transform)
        {
            ChangeLayersRecursively(child.gameObject, layerName);
        }
    }
}
