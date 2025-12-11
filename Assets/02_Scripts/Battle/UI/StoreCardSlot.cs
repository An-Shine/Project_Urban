using UnityEngine;

public class StoreCardSlot : MonoBehaviour
{
    [Header("연결")]
    [SerializeField] private Transform cardContainer; // 카드가 생성될 위치

    private GameObject currentCardObj;

    public void Setup(Card cardPrefab)
    {
        // 1. 기존 카드 제거
        if (cardContainer != null)
        {
            foreach (Transform child in cardContainer) Destroy(child.gameObject);
        }

        // 2. 카드 생성       
        Transform parent = cardContainer != null ? cardContainer : transform;
        currentCardObj = Instantiate(cardPrefab.gameObject, parent);

        // 3. 위치/크기 보정
        currentCardObj.transform.localPosition = new Vector3(0, 0, -300f);
        currentCardObj.transform.localRotation = Quaternion.identity;
        currentCardObj.transform.localScale = Vector3.one * 200f;

        ChangeLayersRecursively(currentCardObj, "UI");
    }

    // 품절 시 회색으로 변하게 하는 함수
    public void SetSoldOutVisual()
    {
        if (currentCardObj != null)
        {
            var renderers = currentCardObj.GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in renderers) r.color = Color.gray;
        }
    }

    private void ChangeLayersRecursively(GameObject target, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer == -1) return;
        target.layer = layer;
        foreach (Transform child in target.transform) ChangeLayersRecursively(child.gameObject, layerName);
    }
}