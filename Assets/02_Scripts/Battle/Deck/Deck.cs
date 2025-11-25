using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // 1. 현재 보유중인 전체 카드 리스트 (게임 전체의 내 덱 정보)
    [SerializeField] List<Card> originCardList = new();

    // 2. 사용하지 않은 카드 (뽑을 수 있는 덱)
    [SerializeField] List<Card> unusedCardList = new();

    // 3. 사용한 카드
    [SerializeField] List<Card> usedCardList = new();

    // 셔플 중복 실행 방지
    private bool isShuffling = false;

    // 덱에 남은 카드 수
    public int UnusedCardCount => unusedCardList.Count;

    // 사용한 카드 수
    public int UsedCardCount =>  usedCardList.Count;


    // [ShuffleLogic] 실제 셔플 로직을 수행하는 내부 함수
    // usedCardList의 카드들을 unusedCardList로 옮긴 후 랜덤하게 섞음
    private void ShuffleLogic()
    {
        // 1. used 에 있는 카드가 없으면 섞을 필요가 없음
        if (usedCardList.Count == 0)
        {
            return;
        }

        // 2. 사용한 카드를 다시 덱으로 회수
        unusedCardList.AddRange(usedCardList);
        usedCardList.Clear();

        // 3. 랜덤하게 섞기
        for (int i = unusedCardList.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            Card temp = unusedCardList[i];
            unusedCardList[i] = unusedCardList[rand];
            unusedCardList[rand] = temp;
        }
    }

    // [Shuffle] 즉시 덱을 섞는다
    // 게임 시작이나 카드 추가 등 딜레이 없이 바로 섞어야 할 때 호출
    public void Shuffle()
    {
        ShuffleLogic();
    }

    // [ShuffleRoutine] 딜레이 후 덱을 섞는 코루틴
    // DrawCard 중 덱이 비었을 때 자연스러운 연출을 위해 사용
    IEnumerator ShuffleRoutine()
    {
        // 중복 실행 방지 잠금
        isShuffling = true;

        // 덱 셔플 애니메이션 시간 대기 (1초)
        yield return new WaitForSeconds(1.0f);

        // 셔플 실행
        ShuffleLogic();

        // 잠금 해제
        isShuffling = false;
    }

    public void SetOriginalDeck(List<Card> prefabList)
    {
        // 1. 기존 리스트 초기화
        originCardList.Clear();
        unusedCardList.Clear();
        usedCardList.Clear();

        // 2. 전달받은 프리팹 리스트를 내 원본 리스트로 복사
        // (이제 originCardList는 '카드 설계도'들을 가지고 있습니다)
        originCardList = new List<Card>(prefabList);       

        // 3. 게임 시작 준비 (원본 -> 뽑을 덱으로 이동 및 셔플)
        ResetDeck();
    }


    // [DrawCard] 카드를 n장 뽑는다
    // 뽑을 수 있는 카드가 부족하면 ShuffleRoutine 실행
    public List<Card> DrawCard(int amount)
    {
        List<Card> drawnCards = new List<Card>();

        for (int i = 0; i < amount; i++)
        {
            // 1. 덱이 비었는지 확인
            if (unusedCardList.Count == 0)
            {
                // 셔플 중이 아니고 덱이 비었다면 셔플 예약 (한 번만 실행)
                if (!isShuffling)
                {
                    StartCoroutine(ShuffleRoutine());
                }
                
                // 카드가 없으므로 더 이상 뽑을 수 없음 -> 중단
                break;
            }

            // 2. 덱의 맨 위 카드를 가져옴
            Card cardToDraw = unusedCardList[0];

            // 3. 덱에서 제거하고 뽑은 목록에 추가
            unusedCardList.RemoveAt(0);
            drawnCards.Add(cardToDraw);

            // 4. 카드를 뽑은 직후, 남은 카드가 0장이라면 셔플 예약
            if (unusedCardList.Count == 0 && !isShuffling)
            {
                StartCoroutine(ShuffleRoutine());
            }
        }

        return drawnCards;
    }


    // [ResetDeck] 덱을 초기 상태로 재설정
    // 게임/스테이지 시작 시 originList를 기반으로 덱을 새로 구성
    public void ResetDeck()
    {
        unusedCardList.Clear();
        usedCardList.Clear();

        // 원본 리스트(Origin)의 내용을 복사해서 unused로 가져옴
        foreach (var card in originCardList)
        {
            unusedCardList.Add(card);
        }

        // 초기 셔플
        Shuffle();
    }


    // [AddCard] 덱(원본, 현재 덱)에 카드를 추가
    // 보상 등으로 카드를 얻었을 때 호출
    public void AddCard(Card card)
    {
        if (card == null) return;

        // 덱(Origin)에 추가
        originCardList.Add(card);
    }


    // [RemoveCard] 덱에서 카드를 제거한다
    // 카드 제거 이벤트/상점 판매
    public void RemoveCard(Card card)
    {
        // 원본 리스트에서 제거
        if (originCardList.Contains(card))
        {
            originCardList.Remove(card);
        }
    }

    // [ChangeCard] 선택한 카드와 덱의 카드를 변경한다
    // oldCard를 제거하고 newCard를 추가
    public void ChangeCard(Card oldCard, Card newCard)
    {
        RemoveCard(oldCard);
        AddCard(newCard);
    }

    // 카드를 사용하고 사용된 카드로 보내는 기능 (외부에서 호출용)
    public void Discard(Card card)
    {
        if (!card.IsException)
            usedCardList.Add(card);
    }
}