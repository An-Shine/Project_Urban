using System.Collections.Generic;
using UnityEngine;

public class ClearRewardUI : MonoBehaviour
{
    [Header("연결")]
    [SerializeField] private RewardCardList[] slots; // 슬롯 3개
    [SerializeField] private GameObject rewardPanel; // UI 전체
    [SerializeField] private GameObject clearRewardObject;  //SelectRewardCard 누르면 꺼지는 오브젝트

    private Deck deck;

    public void OnClickOpenReward()
    { 
        // 1. SelectRewardCard 버튼 그룹 끄기 (화면에서 사라짐)
        if (clearRewardObject != null) 
        {
            clearRewardObject.SetActive(false);
        }

        // 2. 보상카드 생성 로직 실행
        OpenRewardPopup();
    }

    // 보상 팝업 열기 (BattleManager가 호출)
    public void OpenRewardPopup()
    {
        rewardPanel.SetActive(true);

        // 1. Deck 찾기
        if (deck == null)
        {
            var player = FindFirstObjectByType<Player>();
            if (player != null) deck = player.GetComponent<Deck>();
        }

        if (deck == null) return;

        // 2. 현재 속성 가져오기
        Element currentElement = Element.None;
        if (GameManager.Instance != null)
        {
            currentElement = GameManager.Instance.SelectedElement;
        }

        // 3. Deck에게 해당 속성 카드 목록 달라고 요청
        List<CardName> candidateCards = deck.GetCardsByElement(currentElement);

        // 4. 랜덤으로 3장 뽑아서 슬롯에 배치
        for (int i = 0; i < slots.Length; i++)
        {
            if (candidateCards.Count > 0)
            {
                int randomIndex = Random.Range(0, candidateCards.Count);
                CardName pickedName = candidateCards[randomIndex];

                // 프리팹 가져오기
                Card prefab = deck.GetCardPrefab(pickedName);
                
                // 슬롯 세팅
                slots[i].gameObject.SetActive(true);
                slots[i].Setup(this, prefab); // (Setup 함수는 RewardCardList에 정의됨)
            }
            else
            {
                // 후보 카드가 모자라면 슬롯 끄기
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    // 카드 선택 시 (RewardCardList에서 호출)
    public void OnRewardSelected(CardName cardName)
    { 
        // 덱에 추가
        deck.AddCard(cardName);

        // UI 끄기 및 다음 진행
        rewardPanel.SetActive(false);
        // BattleManager.Instance.GoToNextMap(); // (추후 구현)
    }
}