using System;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동을 위해 필수!

public class MapManager : MonoBehaviour
{

    public enum StageType
    {
        Normal,     //기본 몬스터 스테이지
        Shelter,    //휴식 스테이지
        Store,      //상점 스테이지
        Boss        //보스 스테이지
    }

    [Header("UI 패널 연결")]
    [SerializeField] private GameObject shelterUI; // 휴식 UI
    [SerializeField] private GameObject storeUI;   // 상점 UI
    [SerializeField] private GameObject selectElementUI; // 속성 선택 UI
    [SerializeField] private GameObject stageSelectUI;
    [SerializeField] private GameObject CloseStoreUIButton; //상점 UI 닫기버튼

    [SerializeField] private GameObject enchantPopup;    // 카드강화 UI
    [SerializeField] private GameObject removePopup;     // 카드제거 UI
    [SerializeField] private GameObject DeckCheckPanel;  // 덱 확인 UI  
    [SerializeField] private GameObject BuyCardPopup;    // 카드구매 UI
    [SerializeField] private GameObject CardEnchantRemovePanel; //카드강화,제거용 덱확인 패널     
    [SerializeField] private GameObject EnterButton;
    [SerializeField] private GameObject ShelterPopup;
    

    private void Start()
    {        
        if (GameManager.Instance == null) return;        

        bool isReturning = GameManager.Instance.IsStageSelectMode;    

        if (isReturning)
        {            
            if (selectElementUI != null) selectElementUI.SetActive(false);
            if (stageSelectUI != null) stageSelectUI.SetActive(true);
           
            GameManager.Instance.IsStageSelectMode = false;
        }
        else
        {
            if (selectElementUI != null) selectElementUI.SetActive(true);
            if (stageSelectUI != null) stageSelectUI.SetActive(false);
        }
    }

    // 1. Normal 버튼 기능
    public void OnClickNormal()
    {
        SceneManager.LoadScene("Battle_Scene");
    }

    // 2. Shelter 버튼 기능
    public void OnClickShelter()
    {
        shelterUI.SetActive(true);
    }

    // 3. Store 버튼 기능
    public void OnClickStore()
    {
        storeUI.SetActive(true);        
    }

    // 4. Boss 버튼 기능
    public void OnClickBoss()
    {
        SceneManager.LoadScene("Battle_Scene");
    }

    public void OpenEnchantPopup()
    {
        enchantPopup.SetActive(true);        
    }

    // 2. 제거 팝업 열기
    public void OpenRemovePopup()
    {
        removePopup.SetActive(true);
    }

    // 3. 덱 확인 패널 열기
    public void OpenDeckCheckPanel()
    {
        DeckCheckPanel.SetActive(true);
    }

    // 4. 카드 구매 팝업 열기
    public void OpenBuyCardPopup()
    {
        BuyCardPopup.SetActive(true);
    }

    public void OpenCardEnchantRemovePanel()
    {
        CardEnchantRemovePanel.SetActive(true);
    }
    
    public void ClosePopup()
    {        
        transform.parent.gameObject.SetActive(false);        
    }

    public void CloseStoreUI()
    {
        storeUI.SetActive(false);
    }

    public void OpenShelterPopup()
    {
        ShelterPopup.SetActive(true);
        EnterButton.SetActive(false);
    }

    public void CloseShelterPopup()
    {
        ShelterPopup.SetActive(false);
    }
    
    public void CloseShelterPanel()
    {
        shelterUI.SetActive(false);
    }
    
    private void CloseDeckCheckPanel()
    {
        DeckCheckPanel.SetActive(false);
    }
    
}