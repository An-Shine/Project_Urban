using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearRewardUI : MonoBehaviour
{
    [SerializeField] private GameObject clearRewardObject;      // 처음에 떠있는 버튼 그룹 (클릭 시 꺼짐)
    [SerializeField] private Transform rewardCardSpawnPoint; // 카드가 나열될 부모 위치
    [SerializeField] private GameObject rewardSlotPrefab;    // RewardCardList가 붙은 슬롯 프리팹

    [SerializeField] private int rewardCount = 3; // 3장 제시

    private void Start()
    {

    }

    // 보상확인 버튼 클릭시 호출하는 함수
   public void OnClickOpenReward()
    {        
        // 1. 기존 버튼 UI 끄기
        if (clearRewardObject != null) 
            clearRewardObject.SetActive(false);

        // 2. 보상 팝업 로직 실행
        OpenRewardPopup();
    }

    // 실제 카드를 생성하는 함수
    public void OpenRewardPopup()
    {
        if (GameManager.Instance == null)
        {            
            return;
        }

        // GameManager에게 현재 속성의 카드 3장 랜덤 요청
        Element myElement = GameManager.Instance.SelectedElement;
        
        List<Card> rewards = GameManager.Instance.GetRandomCardsByElement(myElement, rewardCount);  
           

        // 슬롯 생성 및 세팅
        foreach (Card cardPrefab in rewards)
        {
            if (rewardSlotPrefab == null)
            {
                break;
            }

            // 슬롯 생성
            GameObject slotObject = Instantiate(rewardSlotPrefab, rewardCardSpawnPoint);
            
            // 슬롯 세팅 (RewardCardList 스크립트 사용)
            RewardCardList slotScript = slotObject.GetComponent<RewardCardList>();
            if (slotScript != null)
            {
                slotScript.Setup(this, cardPrefab);
            }
        }
    }

    // 카드를 골랐을 때 호출 (RewardCardList에서 호출)
    public void OnRewardSelected(CardName cardName)
    {        
        // 1. Player를 찾아서 덱에 추가
        var player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            player.GetComponent<Deck>().AddCard(cardName);
        }        

        // 2. 보상 UI 닫기 (전체 UI 비활성화)
        
        //gameObject.SetActive(false); 
        
        // 예: 맵 선택 화면으로 이동하려면 아래 코드 사용
        SceneManager.LoadScene("Main_UI");
        //SelectElement UI 끄고 StageSelectionButton 키기
    }
}

