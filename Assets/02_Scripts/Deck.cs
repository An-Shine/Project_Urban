using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{    
    // 1. 현재 보유중인 전체 카드 리스트 (게임 전체의 내 덱 정보)
    
    [SerializeField] List<Card> originCardList = new List<Card>();

    // 2. 사용하지 않은 카드 (뽑을 수 있는 덱)
    [SerializeField] List<Card> unusedCardList = new List<Card>();

    // 3. 사용한 카드 (무덤/버린 카드)
    [SerializeField] List<Card> usedCardList = new List<Card>();

   
    // 덱에 남은 카드 수
   
    public int UnusedCardCount
    {
        get { return unusedCardList.Count; }
    }
    
    // 사용한 카드 수
    
    public int UsedCardCount
    {
        get { return usedCardList.Count; }
    }
    
    
    // [Shuffle] 사용된 카드를 섞어서 미사용 카드덱으로 전환
    //  usedCardList의 카드들을 unusedCardList로 옮긴 후 랜덤하게 섞음    
    public void Shuffle()
    {
        // 1.used 에 있는 카드가 없으면 섞을 필요가 없음
        if (usedCardList.Count == 0 && unusedCardList.Count == 0)        {
            
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

    // [DrawCard] 카드를 n장 뽑는다
    // 뽑을 수 있는 카드가 부족하면 Shuffle 실행 후 뽑기   
    public List<Card> DrawCard(int amount)
    {
        List<Card> drawnCards = new List<Card>();

        for (int i = 0; i < amount; i++)
        {
            // 1. 덱이 비었는지 확인
            if (unusedCardList.Count == 0)
            {
                // 셔플 후에도 카드가 없다면 (총 보유 카드가 0장인 경우) 중단
                if (unusedCardList.Count == 0)
                {                    
                    break;
                }
            }

            // 2. 덱의 맨 위 카드를 가져옴
            Card cardToDraw = unusedCardList[0];
            
            // 3. 덱에서 제거하고 뽑은 목록에 추가
            unusedCardList.RemoveAt(0);
            drawnCards.Add(cardToDraw);

            // 4. 카드를뽑았을때, 뽑을수있는카드가 0이라면 1초 뒤 셔플예약
            if (unusedCardList.Count == 0 && !isShuffling)
            {
                StartCoroutine(AutoShuffleRoutine());
            }
        }

        return drawnCards;
    }

    IEnumerator AutoShuffleRoutine()
    {
        // 중복 실행 방지 잠금
        isShuffling = true;         

        //덱 셔플 애니메이션 추가 가능
        yield return new WaitForSeconds(1.0f); 

        //셔플 실행
        ShuffleLogic();

        //잠금 해제
        isShuffling = false;
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

    
    // [AddCard] 덱(원본 및 현재 덱)에 카드를 추가한다
    // 보상 등으로 카드를 얻었을 때 호출
    public void AddCard(Card card)
    {
        if (card == null) return;

        // 덱(Origin)에 추가
        originCardList.Add(card);
        
        // 현재 뽑을 덱(Unused)에도 즉시 추가 (게임 기획에 따라 무덤으로 보낼 수도 있음)
        unusedCardList.Add(card);
        
        // 덱에 새 카드가 들어왔으니 셔플
        Shuffle();     
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

        // 현재 게임 중인 리스트에서도 제거 (어디에 있을지 모르니 둘 다 확인)
        if (unusedCardList.Contains(card)) unusedCardList.Remove(card);
        if (usedCardList.Contains(card)) usedCardList.Remove(card);        
    }
    
    // [ChangeCard] 선택한 카드와 덱의 카드를 변경한다
    // oldCard를 제거하고 newCard를 추가
    public void ChangeCard(Card oldCard, Card newCard)
    {
        RemoveCard(oldCard);
        AddCard(newCard);        
    }
    
    // 카드를 사용하고 무덤으로 보내는 기능 (외부에서 호출용)
    public void Discard(Card card)
    {
        usedCardList.Add(card);
    }
}