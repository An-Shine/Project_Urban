using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/*
List<NodeType> mapTable = new();
for (int i = 0; i < Monster.weight; i++)
    mapTable.Add(Monster);

for (int i = 0; i < Shop.weight; i++)
    mapTable.Add(Shop);

for (int i = 0; i < Event.weight; i++)
    mapTable.Add(Event);

NodeType type = mapTable[Random.Range(0, mapTable.Count)];

if (step == 4)
for (int i = 0; i < Elite.weight; i++)
    mapTable.Add(Elite);

NodeType type = mapTable[Random.Range(0, mapTable.Count)];
NodeType type = mapTable[Random.Range(0, mapTable.Count)];
NodeType type = mapTable[Random.Range(0, mapTable.Count)];

mapTable.RemoveAll(Shop);
NodeType type = mapTable[Random.Range(0, mapTable.Count)];

List<Node> [] map = new();
map[0].Add();
map[9].Add();
map[18].Add();
map[19].Add();

int step = 1;
map[1].Add(), Add(), Add() => 잭팟 검사 => 세개가 같으면 세 중 하나를 랜덤으로 일반 몬스터로 변경

List<Branch> branch = new(width);
class Branch
{
    Dictionary<NodeType, TypeInfo> typeMap;

    public Branch()
    {
        foreach (NodeType type in System.Enum.GetValues(typeof(NodeType)))
        {             
            typeMap[type] = new();
        }
    }
}
*/


public class Branch
{
    private readonly Dictionary<NodeType, NodeTypeDataEntry> nodeMap;

    public Branch(Dictionary<NodeType, NodeTypeDataEntry> nodeMap)
    {
        this.nodeMap = nodeMap;
    }

    public void DecreaseStep()
    {
        foreach (var node in nodeMap)
        {
            NodeTypeDataEntry dataEntry = node.Value;

            if (dataEntry.distanceMax != 0)
                dataEntry.distanceMax--;

            if (dataEntry.distanceMin != 0)
                dataEntry.distanceMin--;
        }
    }

    public void DecreaseSpawnCount(NodeType type)
    {
        if (CanSpawn(type))
            nodeMap[type].maxSpawnCount--;
    }

    public bool CanSpawn(NodeType type)
    {
        return nodeMap[type].maxSpawnCount != 0;
    }
}

public class MapManager : SceneSingleton<MapManager>
{
    [Header("Node Data Reference")]
    [SerializeField] private NodeTypeData nodeTypeData;

    [Header("Branch Settings")]
    [SerializeField] private int width = 3;
    
    private readonly List<Branch> branches = new();
    private readonly List<NodeType> nodeGachaMap = new();
    private Dictionary<NodeType, NodeTypeDataEntry> nodeMap;

    private void Awake()
    {
       InitBranches();
       InitNodeGachaMap();
    }

    private void InitBranches()
    {
         for (int i = 0; i < width; i++)
            branches.Add(new Branch(nodeTypeData.GetNodeMap()));        
    }

    private void InitNodeGachaMap()
    {
        nodeMap = nodeTypeData.GetNodeMap();

        foreach(var entry in nodeMap)
        {
            NodeTypeDataEntry nodeData = entry.Value;
            if (nodeData.minSpawnStep == 0)
            {
                for (int i = 0; i < (int)nodeData.ratio; i++)
                    nodeGachaMap.Add(entry.Key);
            }
        }   
    } 

    private NodeType Gacha()
    {
        return nodeGachaMap[Random.Range(0, nodeGachaMap.Count)];
    }

    private void AddNodeType(NodeType type)
    {
        for (int i = 0; i < nodeMap[type].ratio; i++)
            nodeGachaMap.Add(type);
    }

    private void RemoveNodeType(NodeType type)
    {
        nodeGachaMap.RemoveAll((nodeType) => nodeType == type);
    }
}