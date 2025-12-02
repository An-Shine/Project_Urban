using UnityEngine;
using System.Collections;
using System;

public enum CardType
{
    Attack,  // 공격
    Defense, // 방어
    Buff,    // 버프
    Debuff   // 디버프
}

// Prefab
abstract public class Card : MonoBehaviour
{
    //카드 기본 정보
    [SerializeField] protected Element element;
    [SerializeField] protected int cost;            // 코스트
    
    // 특수 카드 여부
    public bool IsSpecial { get; }

    //제외 카드 여부
    public bool IsException { get; }

    public int Cost => cost;

    abstract public CardName Name { get; }
    abstract public CardType Type { get;}
    abstract public int Use(Target target);

    // 이동 코루틴
    private Coroutine moveCoroutine;

    // 목표 위치로 부드럽게 이동하는 함수
    public void MoveTo(Vector3 targetLocalPos, Action onComplete = null)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveRoutine(targetLocalPos, onComplete));
    }

    private IEnumerator MoveRoutine(Vector3 targetPos, Action onComplete)
    {
        float duration = 0.3f; // 이동 시간
        float time = 0;
        Vector3 startPos = transform.localPosition;

        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(startPos, targetPos, time / duration);       //Lerp 이용해서 부드럽게 이동
            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPos;        //도착 후 정확한 위치로 고정
        moveCoroutine = null;

        // 도착 후 실행할 행동(파괴 등)이 있다면 실행
        onComplete?.Invoke();
    }

    
}