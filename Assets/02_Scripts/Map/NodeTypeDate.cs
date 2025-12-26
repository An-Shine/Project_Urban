using System;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType { Monster, Elite, Store, Shelter, Event, Boss }

[CreateAssetMenu(fileName = "NodeTypeData", menuName = "Node/NodeTypeData", order = 0)]
public class NodeTypeData : ScriptableObject
{
    [SerializeField] private List<NodeTypeDataEntry> nodeEntries = new();
    private readonly Dictionary<NodeType, NodeTypeDataEntry> nodeMap;

    private void OnEnable()
    {
        foreach (NodeTypeDataEntry entry in nodeEntries)
            nodeMap[entry.type] = entry;
    }

    public Dictionary<NodeType, NodeTypeDataEntry> GetNodeMap()
    {
        return new Dictionary<NodeType, NodeTypeDataEntry>(nodeMap);
    }
}

[Serializable]
public class NodeTypeDataEntry
{
    public NodeType type;
    public float ratio;
    public int distanceMin;
    public int distanceMax;
    public int maxSpawnCount;
    public int minSpawnStep;
}