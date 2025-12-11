using UnityEngine;
using System.Collections;
using UnityEngine.Events;

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
    [SerializeField] private Sprite cardImage;
    [SerializeField] protected Element element;
    [SerializeField] protected int cost;            // 코스트
    [SerializeField] protected int price = 100;           // 상점 구매가격
    
    // 이동 코루틴
    private Coroutine moveCoroutine;

    // 카드 정보 관련
    public bool IsSpecial { get; }      // 특수 카드 여부 
    public bool IsException { get; }     //제외 카드 여부

    // Property
    public int Cost => cost;
    public int Price => price;
    public Vector3 OriginPos { get; set; } = new();
    public bool IsEntered { get; set; } = false;
    public Sprite CardImage => cardImage;

    public void Hover()
    {
        transform.localScale *= 1.2f;
        
        Vector3 newPos = OriginPos;
        newPos.z = -2.0f;
        transform.localPosition = newPos;
    }

    public void UnHover()
    {
        transform.localScale /= 1.2f;
        transform.localPosition = OriginPos;
    }

    // 목표 위치로 부드럽게 이동하는 함수
    public void MoveTo(Vector3 targetLocalPos, UnityAction onComplete = null)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        
        OriginPos = targetLocalPos;
        moveCoroutine = StartCoroutine(MoveRoutine(targetLocalPos, onComplete));
    }

    private IEnumerator MoveRoutine(Vector3 targetPos, UnityAction onComplete, float duration = 1.0f)
    {
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

    abstract public CardName Name { get; }
    abstract public CardType Type { get; }
    abstract public int Use(Target target);
}