using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Entities.Navigation
{
    public class NavNode
    {
        public Vector2Int IntPosition;
        public Vector2 Position;
        public Direction Direction;
        public Dictionary<NavNode, float> Connections = new Dictionary<NavNode, float>();
        public readonly Dictionary<NavNode, float> BreakingConnections = new Dictionary<NavNode, float>();
    }
}