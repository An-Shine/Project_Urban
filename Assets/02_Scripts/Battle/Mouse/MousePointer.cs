using UnityEngine;
using System.Collections.Generic;

public class MousePointer : MonoBehaviour
{
    [SerializeField] private float zPosition = 0.0f;
    
    private Camera mainCam;

    // GameObject 
    private GameObject currentHoveredObject = null;
    private GameObject mouseDownObject = null;
    private readonly RaycastHit[] raycastHits = new RaycastHit[5];
    private readonly HashSet<GameObject> previousHoveredObjects = new HashSet<GameObject>();
    private readonly HashSet<GameObject> currentHoveredObjects = new HashSet<GameObject>();

    // Flags
    private bool isMouseDown = false;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        CheckMouseHover();
        CheckMouseClick();
    }

    private void CheckMouseHover()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        GameObject hoveredObject = null;

        int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, 50.0f);

        // 현재 프레임에서 호버된 오브젝트들 초기화
        currentHoveredObjects.Clear();

        if (hitCount > 0)
        {
            // 거리순으로 정렬 (가장 먼 것부터)
            System.Array.Sort(raycastHits, 0, hitCount, 
                Comparer<RaycastHit>.Create((hit1, hit2) => hit2.distance.CompareTo(hit1.distance)));
            
            // 모든 감지된 오브젝트에 대해 이벤트 발생
            for (int i = 0; i < hitCount; i++)
            {
                GameObject hitObject = raycastHits[i].collider.gameObject;
                currentHoveredObjects.Add(hitObject);
                MouseEvents.TriggerMouseHover(hitObject);
            }
            
            // 가장 먼 오브젝트를 현재 호버 오브젝트로 설정
            hoveredObject = raycastHits[0].collider.gameObject;
        }

        // Enter 이벤트: 이전에 없었는데 새로 추가된 오브젝트들
        foreach (GameObject obj in currentHoveredObjects)
        {
            if (!previousHoveredObjects.Contains(obj))
            {
                MouseEvents.TriggerMouseEnter(obj);
            }
        }

        // Exit 이벤트: 이전에 있었는데 현재 없는 오브젝트들
        foreach (GameObject obj in previousHoveredObjects)
        {
            if (!currentHoveredObjects.Contains(obj))
            {
                MouseEvents.TriggerMouseExit(obj);
            }
        }

        // 현재 호버된 오브젝트들을 이전 상태로 저장
        previousHoveredObjects.Clear();
        foreach (GameObject obj in currentHoveredObjects)
        {
            previousHoveredObjects.Add(obj);
        }

        currentHoveredObject = hoveredObject;
    }

    private void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, 50.0f);
            
            if (hitCount > 0)
            {
                // 거리순으로 정렬 (가장 먼 것부터)
                System.Array.Sort(raycastHits, 0, hitCount, 
                    Comparer<RaycastHit>.Create((hit1, hit2) => hit2.distance.CompareTo(hit1.distance)));
                
                // 모든 감지된 오브젝트에 MouseDown 이벤트 발생
                for (int i = 0; i < hitCount; i++)
                {
                    GameObject hitObject = raycastHits[i].collider.gameObject;
                    MouseEvents.TriggerMouseDown(hitObject);
                }
                
                // 가장 먼 오브젝트를 Down 오브젝트로 설정
                mouseDownObject = raycastHits[0].collider.gameObject;
            }
            
            isMouseDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isMouseDown)
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, 50.0f);
                
                if (hitCount > 0)
                {
                    // 거리순으로 정렬 (가장 먼 것부터)
                    System.Array.Sort(raycastHits, 0, hitCount, 
                        Comparer<RaycastHit>.Create((hit1, hit2) => hit2.distance.CompareTo(hit1.distance)));
                    
                    // 모든 감지된 오브젝트에 MouseUp 이벤트 발생
                    for (int i = 0; i < hitCount; i++)
                    {
                        GameObject hitObject = raycastHits[i].collider.gameObject;
                        MouseEvents.TriggerMouseUp(hitObject);
                        
                        // 같은 오브젝트에서 Down->Up된 경우 Click 이벤트도 발생
                        if (hitObject == mouseDownObject)
                        {
                            MouseEvents.TriggerMouseClick(hitObject);
                        }
                    }
                }
            }
            
            isMouseDown = false;
            mouseDownObject = null;
        }
    }

    private void OnDisable()
    {
        // 모든 현재 호버된 오브젝트에 Exit 이벤트 발생
        foreach (GameObject obj in previousHoveredObjects)
        {
            MouseEvents.TriggerMouseExit(obj);
        }
        
        currentHoveredObject = null;
        mouseDownObject = null;
        isMouseDown = false;
        previousHoveredObjects.Clear();
        currentHoveredObjects.Clear();
    }
}