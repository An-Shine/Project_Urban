using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapVisualizer : MonoBehaviour
{
    public static MapVisualizer Instance;

    [Header("UI References")]
    [SerializeField] private Transform mapContentParent; 
    [SerializeField] private GameObject linePrefab;      

    [Header("Node Prefabs")]
    [SerializeField] private GameObject normalStagePrefab;   
    [SerializeField] private GameObject eliteStagePrefab;    
    [SerializeField] private GameObject bossStagePrefab;     
    [SerializeField] private GameObject shelterStagePrefab;  
    [SerializeField] private GameObject storeStagePrefab;    
    
    [Header("Layout Settings")]
    [SerializeField] private float xSpacing = 200f;      // 노드 간 가로 간격
    [SerializeField] private float ySpacing = 150f;      // 노드 간 세로 간격
    [SerializeField] private float bottomPadding = 100f; // 1층 노드가 바닥에서 얼마나 띄워질지

    // 선 과 노드를 담을 별도의 부모 객체 (계층 구조 분리)

    private Transform lineContainer;
    private Transform nodeContainer;

    // 데이터와 실제 생성된 게임오브젝트를 짝지어 저장하는 딕셔너리
    private Dictionary<MapNode, GameObject> nodeObjMap = new Dictionary<MapNode, GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    // 맵 데이터를 받아 화면에 그리는 함수
  public void ShowMap(List<List<MapNode>> mapGrid)
    {
        RectTransform contentRect = mapContentParent.GetComponent<RectTransform>();
        contentRect.pivot = new Vector2(0.5f, 0f); 
        contentRect.anchorMin = new Vector2(0.5f, 0f);
        contentRect.anchorMax = new Vector2(0.5f, 0f);
        
        // 위치를 0,0으로 초기화
        contentRect.anchoredPosition = Vector2.zero; 

        // 2. 초기화
        foreach (Transform child in mapContentParent) Destroy(child.gameObject);
        nodeObjMap.Clear();

        CreateContainer("LineContainer", out lineContainer);
        CreateContainer("NodeContainer", out nodeContainer);

        // 3. 노드 생성
        int mapWidth = mapGrid[0].Count;
        float topPadding = 200f; // 보스 위 여유 공간
        float contentHeight = (mapGrid.Count * ySpacing) + bottomPadding + topPadding;
        
        // 계산된 높이 적용
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);

        // 4. 노드 배치 루프
        for (int y = 0; y < mapGrid.Count; y++)
        {
            List<MapNode> row = mapGrid[y];
            for (int x = 0; x < row.Count; x++)
            {
                MapNode node = row[x];
                if (node.isActive)
                {
                    CreateNodeObject(node, mapWidth);
                }
            }
        }

        // 5. 연결선 그리기
        DrawConnections();
        
        // 6. 스크롤 위치 초기화
        StartCoroutine(ResetScroll());
    }

    private void CreateNodeObject(MapNode node, int mapWidth)
    {
        GameObject prefabToUse = GetPrefabByNodeType(node.nodeType);
        if (prefabToUse == null) return;

        GameObject newObj = Instantiate(prefabToUse, nodeContainer);
        newObj.transform.localScale = Vector3.one;

        // X축: 중앙 정렬
        float xPos = (node.x - (mapWidth - 1) / 2.0f) * xSpacing;
        
        // Y축: 바닥(0) 기준 좌표 계산
        float yPos = bottomPadding + (node.y * ySpacing); 

        newObj.transform.localPosition = new Vector3(xPos, yPos, 0);
        
        nodeObjMap.Add(node, newObj);
    }


    // 노드나 선을 담을 투명한 부모 객체폴더를 만드는 함수
   private void CreateContainer(string name, out Transform container)
    {
        // 1. 오브젝트 생성
        GameObject go = new GameObject(name);
        go.transform.SetParent(mapContentParent, false); 
        
        // 2. RectTransform 컴포넌트 추가
        RectTransform rect = go.AddComponent<RectTransform>();
        
        // 3. 앵커설정
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;

        // 노드 생성 영역 설정
        rect.offsetMin = new Vector2(0f, -1500f); 
        rect.offsetMax = new Vector2(0f, -1500f); 

        // 스케일과 Z좌표 초기화
        rect.localScale = Vector3.one;
        rect.anchoredPosition3D = new Vector3(rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0f);
        
        container = go.transform;
    }   

    // 생성된 노드들을 순회하며 선을 긋는 함수
    private void DrawConnections()
    {
        foreach (var kvp in nodeObjMap)
        {
            MapNode currentNode = kvp.Key;
            // 현재 노드의 화면상 위치
            Vector3 startPos = kvp.Value.transform.localPosition;

            // 이 노드와 연결된 다음 층 노드들을 모두 확인
            foreach (MapNode nextNode in currentNode.nextNodes)
            {
                // 다음 노드가 생성되어 있다면
                if (nodeObjMap.TryGetValue(nextNode, out GameObject nextObj))
                {
                    // 두 지점 사이에 선 그리기
                    CreateLine(startPos, nextObj.transform.localPosition);
                }
            }
        }
    }

    /// 두 점(start, end) 사이에 직선 이미지를 생성하고 회전시키는 함수
    private void CreateLine(Vector3 startPos, Vector3 endPos)
    {
        // 라인 컨테이너의 자식으로 생성
        GameObject line = Instantiate(linePrefab, lineContainer);
        
        RectTransform rect = line.GetComponent<RectTransform>();
        // 선의 중심점을 기준으로 회전 및 길이 조절을 위해 피벗을 (0.5, 0.5)로 설정
        rect.pivot = new Vector2(0.5f, 0.5f); 

        // 1. 방향 벡터 (도착점 - 시작점)
        Vector3 dir = (endPos - startPos).normalized;
        
        // 2. 두 점 사이의 거리 구하기
        float distance = Vector3.Distance(startPos, endPos);

        // 3. 위치 설정: 두 점의 '중간 지점'에 선을 둠
        rect.localPosition = startPos + (dir * distance * 0.5f);
        
        // 4. 회전 설정
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rect.localRotation = Quaternion.Euler(0, 0, angle);
        
        // 5. 크기 설정: 너비를 거리만큼 늘림
        rect.sizeDelta = new Vector2(distance, rect.sizeDelta.y);
    }

    /// 노드 타입에 맞춰 연결된 프리팹을 반환
    private GameObject GetPrefabByNodeType(NodeType type)
    {
        switch (type)
        {
            case NodeType.Monster: return normalStagePrefab;
            case NodeType.Elite:   return eliteStagePrefab;
            case NodeType.Boss:    return bossStagePrefab;
            case NodeType.Shelter: return shelterStagePrefab;
            case NodeType.Store:   return storeStagePrefab;
            
            default: return null;
        }
    }
    
    /// UI 갱신 타이밍 문제 해결을 위해 1프레임 대기 후 스크롤을 내리는 코루틴
    private IEnumerator ResetScroll()
    {
        // UI가 완전히 그려질 때까지 한 프레임 대기
        yield return null; 
        
        // Scroll View 컴포넌트를 찾아서
        ScrollRect sr = mapContentParent.GetComponentInParent<ScrollRect>();
       
        sr.verticalNormalizedPosition = 0f; 
        // 스크롤이 튕기는 관성 효과 제거 (딱 멈추게)
        sr.velocity = Vector2.zero;
        
    }
}