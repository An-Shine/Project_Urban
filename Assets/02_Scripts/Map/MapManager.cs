using System.Collections.Generic;
using UnityEngine;

// 1. 노드 타입 정의
public enum NodeType { Monster = 0, Elite, Store, Shelter, Event, Boss, None }

// 2. 맵 노드 데이터 클래스
[System.Serializable]
public class MapNode
{
    public int x, y;
    public NodeType nodeType = NodeType.None;
    public bool isActive = false;
    
    public List<MapNode> nextNodes = new List<MapNode>(3);
}

// 3. 생성 규칙 데이터
[System.Serializable]
public class MapGenRule
{
    public NodeType type;
    public float ratio;          // 등장 확률
    public int distanceMin;      // 최소 간격 (쿨타임)
    public int distanceMax;      // 최대 간격 (천장)
    public int maxSpawnCount;    // 최대 등장 횟수
    public int minSpawnStep;     // 등장 시작 층
}


public class Branch
{
    private readonly Dictionary<NodeType, MapGenRule> ruleBook;
    private int[] lastSeenStep; 
    private int[] spawnCount;
    private int typeCount; 

    public Branch(Dictionary<NodeType, MapGenRule> rules)
    {
        this.ruleBook = rules;
        
        // Enum의 개수를 파악하여 배열 크기 결정
        this.typeCount = System.Enum.GetValues(typeof(NodeType)).Length;
        
        // 배열 할당 
        this.lastSeenStep = new int[typeCount];
        this.spawnCount = new int[typeCount];
        
        ResetState();
    }

    private void ResetState()
    {
        for (int i = 0; i < typeCount; i++)
        {
            lastSeenStep[i] = -10; // 초기값 
            spawnCount[i] = 0;
        }
    }

    // 2막 진입 시 데이터 계승
    public void InheritData(Branch oldBranch)
    {
        // 루프를 돌리는 것보다 System.Array.Copy가 내부적으로 더 빠릅니다.
        System.Array.Copy(oldBranch.lastSeenStep, this.lastSeenStep, typeCount);
    }

    // 생성 가능 여부 체크
    public bool CanSpawn(NodeType type, int currentFloor)
    {
        if (!ruleBook.TryGetValue(type, out var rule)) return false;
        
        int idx = (int)type; 

        if (rule.maxSpawnCount != -1 && spawnCount[idx] >= rule.maxSpawnCount) return false;
        if (currentFloor < rule.minSpawnStep) return false;
        
        // 쿨타임 체크
        if (currentFloor - lastSeenStep[idx] <= rule.distanceMin) return false;

        return true;
    }

    // 천장(Pity) 체크
    public bool CheckPity(NodeType type, int currentFloor)
    {
        if (!ruleBook.TryGetValue(type, out var rule)) return false;
        
        int idx = (int)type; // 인덱스 접근

        if (rule.maxSpawnCount != -1 && spawnCount[idx] >= rule.maxSpawnCount) return false;
        if (currentFloor < rule.minSpawnStep) return false;
        if (rule.distanceMax <= 0) return false;

        int lastSeen = (lastSeenStep[idx] < 0) ? 0 : lastSeenStep[idx];
        return (currentFloor - lastSeen >= rule.distanceMax);
    }

    // 노드 확정 시 기록
    public void OnNodeSelected(NodeType type, int currentFloor)
    {
        int idx = (int)type;
        lastSeenStep[idx] = currentFloor;
        spawnCount[idx]++;
    }

    // 잭팟 방지 롤백
    public void ModifyHistory(NodeType oldType, NodeType newType, int currentFloor)
    {
        int oldIdx = (int)oldType;
        if (spawnCount[oldIdx] > 0) spawnCount[oldIdx]--;

        // 새 타입 기록 
        OnNodeSelected(newType, currentFloor);
    }
}

// 5. 맵 매니저
public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("Map Dimensions")]
    [SerializeField] private int width = 3;
    [SerializeField] private int height = 20;

    // 맵 그리드 데이터
    public List<List<MapNode>> mapGrid;

    private List<Branch> branches;
    private Dictionary<NodeType, MapGenRule> earlyRules = new Dictionary<NodeType, MapGenRule>();
    private Dictionary<NodeType, MapGenRule> lateRules = new Dictionary<NodeType, MapGenRule>();

    private void Awake()
    {
        Instance = this;
        InitRuleData();
        InitBranches();
    }

    private void Start()
    {
        GenerateMap();
    }

    private void InitRuleData()
    {
        // 룰 설정 함수 
        void AddRule(Dictionary<NodeType, MapGenRule> dict, NodeType type, float ratio, int dMin, int dMax, int maxSpawn, int minStep)
        {
            dict.Add(type, new MapGenRule { 
                type = type, ratio = ratio, distanceMin = dMin, distanceMax = dMax, maxSpawnCount = maxSpawn, minSpawnStep = minStep 
            });
        }
        
        // 전반부 규칙
        earlyRules.Clear();
        AddRule(earlyRules, NodeType.Monster, 70, 0, 0, -1, 1);
        AddRule(earlyRules, NodeType.Elite,   10, 1, 3, -1, 4);
        AddRule(earlyRules, NodeType.Store,   5,  1, 4, 2,  1);
        AddRule(earlyRules, NodeType.Shelter, 5,  2, 6, 2,  5);
        AddRule(earlyRules, NodeType.Event,   10, 1, 4, 3,  1);

        // 후반부 규칙
        lateRules.Clear();
        AddRule(lateRules, NodeType.Monster, 60, 0, 0, -1, 1);
        AddRule(lateRules, NodeType.Elite,   15, 1, 3, -1, 1);
        AddRule(lateRules, NodeType.Store,   10, 1, 4, 2,  1);
        AddRule(lateRules, NodeType.Shelter, 5,  2, 6, 2,  1);
        AddRule(lateRules, NodeType.Event,   10, 1, 4, 3,  1);
    }

    private void InitBranches()
    {
        // 리스트 초기 크기 지정
        branches = new List<Branch>(width);
        for (int i = 0; i < width; i++)
        {
            branches.Add(new Branch(earlyRules));
        }
    }

    // 맵 생성 메인 함수 
    public void GenerateMap()
    {
        // 1. 그리드 초기화 
        mapGrid = new List<List<MapNode>>(height);
        for (int y = 0; y < height; y++)
        {
            // 각 행(row)도 가로 폭(width)만큼
            List<MapNode> row = new List<MapNode>(width);
            for (int x = 0; x < width; x++) row.Add(new MapNode { x = x, y = y });
            mapGrid.Add(row);
        }

        List<NodeType> floorTypes = new List<NodeType>(width);
        Dictionary<NodeType, MapGenRule> currentRules = earlyRules;

        // 2. 층별 노드 결정
        for (int floor = 1; floor <= height; floor++)
        {
             int floorIndex = floor - 1;

            // 11층 규칙 변경 및 데이터 계승
            if (floor == 11)
            {
                currentRules = lateRules;
                List<Branch> oldBranches = new List<Branch>(branches);
                branches.Clear();
                for(int i=0; i<width; i++) 
                {
                    Branch newBranch = new Branch(lateRules);
                    newBranch.InheritData(oldBranches[i]); 
                    branches.Add(newBranch);
                }
            }

            floorTypes.Clear();
            bool isSingleNodeFloor = (floor == 10 || floor == 19 || floor == 20);
            int centerIndex = width / 2; 

            for (int x = 0; x < width; x++)
            {
                MapNode node = mapGrid[floorIndex][x];

                // 외길 층 처리
                if (isSingleNodeFloor && x != centerIndex)
                {
                    node.nodeType = NodeType.None;
                    node.isActive = false;
                    continue;
                }

                // 타입 결정
                NodeType selectedType = DetermineNodeType(branches[x], floor, currentRules);
                
                node.nodeType = selectedType;
                node.isActive = true;
                
                floorTypes.Add(selectedType);
                branches[x].OnNodeSelected(selectedType, floor);
            }

            // 잭팟 방지
            if (!isSingleNodeFloor)
            {
                CheckAndFixJackpot(floorIndex, floor, floorTypes);
            }
        }
        
        // 3. 연결 생성 
        GenerateFixedPaths();

        // 4. 시각화 요청
        if (MapVisualizer.Instance != null)
            MapVisualizer.Instance.ShowMap(mapGrid);
    }

    // 노드 타입 결정 
    private NodeType DetermineNodeType(Branch branch, int floor, Dictionary<NodeType, MapGenRule> rules)
    {
        // 고정 층
        if (floor == 1) return NodeType.Monster;
        if (floor == 10) return NodeType.Shelter;
        if (floor == 19) return NodeType.Shelter;
        if (floor == 20) return NodeType.Boss;

        // 천장 체크
        List<NodeType> pityList = new List<NodeType>();
        foreach (var key in rules.Keys)
        {
            if (branch.CheckPity(key, floor)) pityList.Add(key);
        }
        if (pityList.Count > 0) return pityList[Random.Range(0, pityList.Count)];

        // 가중치 랜덤
        float totalWeight = 0f;
        foreach (var entry in rules.Values)
        {
            if (branch.CanSpawn(entry.type, floor)) totalWeight += entry.ratio;
        }

        if (totalWeight <= 0) return NodeType.Monster;

        float randomValue = Random.Range(0, totalWeight);
        foreach (var entry in rules.Values)
        {
            if (branch.CanSpawn(entry.type, floor))
            {
                randomValue -= entry.ratio;
                if (randomValue <= 0) return entry.type;
            }
        }

        return NodeType.Monster;
    }

    // 잭팟 방지 
    private void CheckAndFixJackpot(int floorIndex, int currentFloor, List<NodeType> types)
    {
        if (types.Count >= 3)
        {
            NodeType first = types[0];
            if (first != NodeType.Monster)
            {
                bool allSame = true;
                for(int i=1; i<types.Count; i++)
                {
                    if(types[i] != first) { allSame = false; break; }
                }

                if (allSame)
                {
                    int targetIdx = Random.Range(0, width);
                    mapGrid[floorIndex][targetIdx].nodeType = NodeType.Monster;
                    branches[targetIdx].ModifyHistory(first, NodeType.Monster, currentFloor);
                }
            }
        }
    }

    // 고정 패턴 연결 로직
    private void GenerateFixedPaths()
    {
        // 1. 연결 정보 초기화
        foreach (var row in mapGrid)
            foreach (var node in row)
            {
                node.nextNodes.Clear(); 
            }

        // 2. 층을 올라가며 연결
        for (int y = 0; y < height - 1; y++)
        {
            for (int x = 0; x < width; x++)
            {
                MapNode currentNode = mapGrid[y][x];
                if (!currentNode.isActive) continue;

                // 직선 구간
                if ((y >= 0 && y <= 7) || (y >= 10 && y <= 16))
                {
                    TryConnect(currentNode, x, y + 1); 
                }
                // 모이는 구간
                else if (y == 8 || y == 17)
                {
                    TryConnect(currentNode, 1, y + 1); 
                }
                // 퍼지는 구간
                else if (y == 9)
                {
                    if (x == 1) 
                    {
                        TryConnect(currentNode, 0, y + 1);
                        TryConnect(currentNode, 1, y + 1);
                        TryConnect(currentNode, 2, y + 1);
                    }
                }
                // 보스 직전 직선
                else if (y == 18)
                {
                    if (x == 1) TryConnect(currentNode, 1, y + 1);
                }
            }
        }
    }

    private void TryConnect(MapNode fromNode, int targetX, int targetY)
    {
        if (targetY >= height || targetX < 0 || targetX >= width) return;
        
        MapNode targetNode = mapGrid[targetY][targetX];
        if (!targetNode.isActive) return;
        fromNode.nextNodes.Add(targetNode);
    }
}