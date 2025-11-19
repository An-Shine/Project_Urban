using UnityEngine;
using UnityEngine.Events;

public static class MouseEvents
{
    // 마우스 호버 이벤트
    public static UnityEvent<GameObject> OnMouseEnter = new();
    public static UnityEvent<GameObject> OnMouseExit = new();
    public static UnityEvent<GameObject> OnMouseHover = new();
    
    // 마우스 클릭 이벤트
    public static UnityEvent<GameObject> OnMouseClick = new();
    public static UnityEvent<GameObject> OnMouseDown = new();
    public static UnityEvent<GameObject> OnMouseUp = new();
    
    // 마우스 위치 이벤트
    public static UnityEvent<Vector3> OnMouseMove = new();

    // 이벤트 발생 메서드들
    public static void TriggerMouseEnter(GameObject obj) => OnMouseEnter?.Invoke(obj);
    public static void TriggerMouseExit(GameObject obj) => OnMouseExit?.Invoke(obj);
    public static void TriggerMouseHover(GameObject obj) => OnMouseHover?.Invoke(obj);
    public static void TriggerMouseClick(GameObject obj) => OnMouseClick?.Invoke(obj);
    public static void TriggerMouseDown(GameObject obj) => OnMouseDown?.Invoke(obj);
    public static void TriggerMouseUp(GameObject obj) => OnMouseUp?.Invoke(obj);
    public static void TriggerMouseMove(Vector3 worldPos) => OnMouseMove?.Invoke(worldPos);
}