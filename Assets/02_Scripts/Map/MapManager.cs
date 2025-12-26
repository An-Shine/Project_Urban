using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum NodeType { None, Monster, Elite, Store, Shelter, Event, Treasure, Boss }

public class MapNode
{
    public int x, y;
    public NodeType nodeType = NodeType.None;
    public List<MapNode> nextNodes = new List<MapNode>();
    public List<MapNode> prevNodes = new List<MapNode>();
    public bool isActive = false;
}

[System.Serializable]
public class GenRule
{
    public NodeType type;       // 방 종류
    [Range(0, 100)] 
    public int weight;          // 등장 확률
    public int minDist;         // 최소 간격
    public int maxDist;         // 최대 간격
    public int maxSpawnCount;   // 등장 횟수 제한 (-1은 무제한)
    public int minFloorLimit;   // 최소 등장 층
}

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("Map Settings")]
    public int mapSeed = 0;
    public bool useRandomSeed = true;
    public int mapWidth = 3;
    public int mapHeight = 20;

    [Header("Rule Settings")]
    [SerializeField] private List<GenRule> earlyRules = new List<GenRule>(); // 2~9층
    [SerializeField] private List<GenRule> lateRules = new List<GenRule>();  // 11~18층

    private List<List<MapNode>> mapGrid;
    private Dictionary<NodeType, int> lastSeenFloor = new Dictionary<NodeType, int>();
    private Dictionary<NodeType, int> currentPhaseSpawnCounts = new Dictionary<NodeType, int>();

    private void Awake()
    {
        Instance = this;
        
        InitializeRules();

        // 2. 상태 초기화
        foreach (NodeType t in System.Enum.GetValues(typeof(NodeType)))
        {             
            lastSeenFloor[t] = 0; 
            currentPhaseSpawnCounts[t] = 0;
        }
    }

    private void Start()
    {
        GenerateMap();
    }

    private void InitializeRules()
    {
        earlyRules.Clear();
        lateRules.Clear();

        // === 전반부 (2~9층) ===
        // 일반 전투 (70%)
        earlyRules.Add(new GenRule { type = NodeType.Monster, weight = 70, minDist = 0, maxDist = 0, maxSpawnCount = -1, minFloorLimit = 0 });
        // 정예 (10%, 4층부터)
        earlyRules.Add(new GenRule { type = NodeType.Elite, weight = 10, minDist = 1, maxDist = 3, maxSpawnCount = -1, minFloorLimit = 4 });
        // 상점 (5%)
        earlyRules.Add(new GenRule { type = NodeType.Store, weight = 5, minDist = 1, maxDist = 4, maxSpawnCount = 2, minFloorLimit = 0 });
        // 휴식처 (5%, 5층부터)
        earlyRules.Add(new GenRule { type = NodeType.Shelter, weight = 5, minDist = 2, maxDist = 6, maxSpawnCount = 2, minFloorLimit = 5 });
        // 이벤트 (10%)
        earlyRules.Add(new GenRule { type = NodeType.Event, weight = 10, minDist = 1, maxDist = 4, maxSpawnCount = 3, minFloorLimit = 0 });

        // === 후반부 (11~18층) ===
        // 일반 전투 (60%)
        lateRules.Add(new GenRule { type = NodeType.Monster, weight = 60, minDist = 0, maxDist = 0, maxSpawnCount = -1, minFloorLimit = 0 });
        // 정예(15%)
        lateRules.Add(new GenRule { type = NodeType.Elite, weight = 15, minDist = 1, maxDist = 3, maxSpawnCount = -1, minFloorLimit = 0 });
        // 상점 (10%)
        lateRules.Add(new GenRule { type = NodeType.Store, weight = 10, minDist = 1, maxDist = 4, maxSpawnCount = 2, minFloorLimit = 0 });
        // 휴식처 (5%)
        lateRules.Add(new GenRule { type = NodeType.Shelter, weight = 5, minDist = 2, maxDist = 6, maxSpawnCount = 2, minFloorLimit = 0 });
        // 이벤트 (10%)
        lateRules.Add(new GenRule { type = NodeType.Event, weight = 10, minDist = 1, maxDist = 4, maxSpawnCount = 3, minFloorLimit = 0 });
    }

    public void GenerateMap()
    {
        if (useRandomSeed) mapSeed = Random.Range(0, int.MaxValue);
        Random.InitState(mapSeed);

        CreateGrid();
        GeneratePaths();
        AssignNodeTypes();

        MapVisualizer.Instance.ShowMap(mapGrid);
    }

    void CreateGrid()
    {
        mapGrid = new List<List<MapNode>>(mapHeight);
        for (int y = 0; y < mapHeight; y++)
        {
            List<MapNode> row = new List<MapNode>(mapWidth);
            for (int x = 0; x < mapWidth; x++)
            {
                row.Add(new MapNode { x = x, y = y });
            }
            mapGrid.Add(row);
        }
    }

    // 3라인 유지 방식
    void GeneratePaths()
    {
        // 1. 초기화
        foreach (var row in mapGrid)
            foreach (var node in row)
            {
                node.isActive = false;
                node.nextNodes.Clear();
                node.prevNodes.Clear();
            }

        // 2. 3개의 워커 독립 관리
        int[] walkerX = new int[mapWidth]; 
        for (int i = 0; i < mapWidth; i++) walkerX[i] = i; 

        for (int i = 0; i < mapWidth; i++) mapGrid[0][walkerX[i]].isActive = true;

        for (int y = 0; y < mapHeight - 1; y++)
        {
            int[] nextWalkerX = new int[mapWidth];
            for (int i = 0; i < mapWidth; i++)
            {
                int currentX = walkerX[i];
                int dir = Random.Range(-1, 2);
                int nextX = Mathf.Clamp(currentX + dir, 0, mapWidth - 1);
                
                nextWalkerX[i] = nextX;
                
                MapNode currentNode = mapGrid[y][currentX];
                MapNode nextNode = mapGrid[y + 1][nextX];
                
                ConnectNodes(currentNode, nextNode);
            }
            walkerX = nextWalkerX;
        }

        // 3. 노드 연결 
        for (int y = 0; y < mapHeight - 2; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                MapNode node = mapGrid[y][x];
                if (!node.isActive) continue;

                if (Random.value < 0.2f) 
                {
                    int targetX = (Random.value < 0.5f) ? x - 1 : x + 1;
                    if (targetX >= 0 && targetX < mapWidth)
                    {
                        ConnectNodes(node, mapGrid[y + 1][targetX]);
                    }
                }
            }
        }

        // 4. 보스방 집결
        MapNode bossNode = mapGrid[mapHeight - 1][mapWidth / 2];
        bossNode.isActive = true;
        for (int x = 0; x < mapWidth; x++)
        {
            MapNode node = mapGrid[mapHeight - 2][x];
            if (node.isActive) ConnectNodes(node, bossNode);
        }
    }

    void ConnectNodes(MapNode from, MapNode to)
    {
        if (!from.nextNodes.Contains(to))
        {
            from.nextNodes.Add(to);
            to.prevNodes.Add(from);
            from.isActive = true;
            to.isActive = true;
        }
    }

    void AssignNodeTypes()
    {
        foreach (NodeType t in System.Enum.GetValues(typeof(NodeType))) lastSeenFloor[t] = 0;

        for (int y = 0; y < mapHeight; y++)
        {
            int floorNum = y + 1;
            if (floorNum == 11)
            {
                var keys = new List<NodeType>(currentPhaseSpawnCounts.Keys);
                foreach (var key in keys) currentPhaseSpawnCounts[key] = 0;
            }

            List<MapNode> row = mapGrid[y];
            List<MapNode> activeNodes = new List<MapNode>();
            foreach(var n in row) if(n.isActive) activeNodes.Add(n);
            
            for (int i = 0; i < activeNodes.Count; i++)
            {
                MapNode temp = activeNodes[i];
                int r = Random.Range(i, activeNodes.Count);
                activeNodes[i] = activeNodes[r];
                activeNodes[r] = temp;
            }

            foreach (var node in activeNodes)
            {
                node.nodeType = DetermineNodeType(floorNum, node);
            }
        }
    }

    NodeType DetermineNodeType(int floor, MapNode node)
    {
        if (floor == 1) return NodeType.Monster;
        if (floor == 10 || floor == 19) return NodeType.Shelter;
        if (floor == 20) return NodeType.Boss;

        List<GenRule> currentRules = (floor < 10) ? earlyRules : lateRules;
        List<GenRule> candidates = new List<GenRule>();
        List<GenRule> pityCandidates = new List<GenRule>();

        foreach (var rule in currentRules)
        {
            if (rule.maxSpawnCount != -1 && currentPhaseSpawnCounts[rule.type] >= rule.maxSpawnCount) continue;
            if (floor < rule.minFloorLimit) continue;
            if (floor - lastSeenFloor[rule.type] <= rule.minDist) continue;

            bool siblingConflict = false;
            foreach (var parent in node.prevNodes)
            {
                foreach (var sibling in parent.nextNodes)
                {
                    if (sibling == node) continue;
                    if (sibling.nodeType == rule.type)
                    {
                        siblingConflict = true;
                        break;
                    }
                }
                if (siblingConflict) break;
            }
            if (siblingConflict && rule.type != NodeType.Monster) continue;

            candidates.Add(rule);

            if (floor - lastSeenFloor[rule.type] > rule.maxDist)
            {
                pityCandidates.Add(rule);
            }
        }

        NodeType selectedType = NodeType.Monster;

        if (pityCandidates.Count > 0)
        {
            selectedType = pityCandidates[Random.Range(0, pityCandidates.Count)].type;
        }
        else if (candidates.Count > 0)
        {
            int totalWeight = candidates.Sum(r => r.weight);
            int rnd = Random.Range(0, totalWeight);
            int currentWeight = 0;
            foreach (var rule in candidates)
            {
                currentWeight += rule.weight;
                if (rnd < currentWeight)
                {
                    selectedType = rule.type;
                    break;
                }
            }
        }

        lastSeenFloor[selectedType] = floor;
        currentPhaseSpawnCounts[selectedType]++;

        return selectedType;
    }

    public List<List<MapNode>> GetMap() => mapGrid;
}