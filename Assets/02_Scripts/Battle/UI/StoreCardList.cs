using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreCardList : MonoBehaviour
{
    [Header("UI 연결용")]
    [SerializeField] private Transform cardSpawnPoint;  //카드프리펩 생성위치 지정
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button rerollButton;

    private StoreUI storeManager;
    private CardName cardName;
    private int cardPrice;
    private GameObject currentCardObject;
    
    public void StoreSetting(StoreUI manager, Card cardPrefab, int price)
    {
        storeManager = manager;
        cardName = cardPrefab.Name;
        cardPrice = price;

        // 1. 카드 프리펩 생성
        currentCardObject = Instantiate(cardPrefab.gameObject, cardSpawnPoint);
        currentCardObject.transform.localPosition = Vector3.zero;

        // 위치, 회전, 크기 초기화
        currentCardObject.transform.localPosition = new Vector3(0, 0, -100f);
        
        // 회전 정렬
        currentCardObject.transform.localRotation = Quaternion.identity;
        
        // 크기 조정
        currentCardObject.transform.localScale = new Vector3(130.0f, 200.0f, 1.0f);
        ChangeLayersRecursively(currentCardObject, "UI");

        UnityEngine.Rendering.SortingGroup sg = currentCardObject.GetComponent<UnityEngine.Rendering.SortingGroup>();
        if (sg != null)
        {
            sg.sortingLayerName = "Default"; // UI용 레이어가 있다면 변경
            sg.sortingOrder = 1000; // 무조건 맨 위로
        }
        else
        {
            // SortingGroup이 없다면 SpriteRenderer라도 찾아서 올림
            var sr = currentCardObject.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sortingOrder = 1000;
        }

        // 2. 텍스트 설정
        priceText.text = $"{cardPrice} G";

        // 3. 구매버튼 연결
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuy);

        // 4. 리롤버튼 설정
        rerollButton.onClick.RemoveAllListeners();
        rerollButton.onClick.AddListener(OnReroll);

        // 5. 버튼 활성화
        buyButton.interactable = true;
        rerollButton.interactable = true;
    }

    private void OnBuy()
    {
        storeManager.BuyCard(cardName, cardPrice, this);
    }

    private void OnReroll()
    {
        storeManager.RerollSlot(this);
    }

    public void SetSoldOut()
    {
        priceText.text = "Sold OUt";
        buyButton.interactable = false;
        rerollButton.interactable = false;
    }

    // 자식오브젝트까지 UI 레이어로 변경
    private void ChangeLayersRecursively(GameObject target, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer == -1) return; // 레이어가 없으면 패스

        target.layer = layer;
        foreach (Transform child in target.transform)
        {
            ChangeLayersRecursively(child.gameObject, layerName);
        }
    }



}
