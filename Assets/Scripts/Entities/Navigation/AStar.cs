using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entities.Navigation
{
    public static class AStar
    {
        public static List<NavNode> FindPath(NavTree tree,
            Vector2 start,
            Vector2 target)
        {
            var startNode = new NavNode();
            startNode.Position = start;
            foreach (var child in tree.Nodes) {
                if (tree.AbleToMove(start, child.Position)) {
                    startNode.Connections.Add(child, Vector2.Distance(start, child.Position));
                }
            }
            
            var connectedToTarget = new Dictionary<NavNode, float>();
            
            var targetNode = new NavNode();
            targetNode.Position = target;
            foreach (var child in tree.Nodes.Append(startNode)) {
                if (tree.AbleToMove(target, child.Position)) {
                    connectedToTarget.Add(child, Vector2.Distance(target, child.Position));
                }
            }
            
            
            // Узлы, которые нужно рассмотреть.
            var openSet = new List<NavNode>();

            // Узлы, которые уже полностью обработаны.
            var closedSet = new HashSet<NavNode>();

            // Стоимость пути от start до конкретного узла.
            var gScore = new Dictionary<NavNode, float>();

            // Предполагаемая стоимость оставшегося пути.
            var fScore = new Dictionary<NavNode, float>();

            // Откуда мы пришли в данный узел.
            var cameFrom = new Dictionary<NavNode, NavNode>();

            gScore[startNode] = 0f;
            fScore[startNode] = Heuristic(startNode, targetNode);

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                // Находим узел с минимальным F.
                NavNode current = GetLowestFScore(openSet, fScore);

                // Мы дошли до цели.
                if (current == targetNode)
                    return ReconstructPath(cameFrom, current);

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (NavNode neighbour in connectedToTarget.ContainsKey(current) ? current.Connections.Keys.Append(targetNode) : current.Connections.Keys)
                {
                    if (closedSet.Contains(neighbour))
                        continue;

                    float tentativeG;
                    if (neighbour == targetNode) {
                        tentativeG = gScore[current] + connectedToTarget[current];
                    }
                    else {
                        tentativeG = gScore[current] + current.Connections[neighbour];
                    }
                       

                    // Мы нашли более дешёвый путь до neighbour.
                    if (!gScore.TryGetValue(neighbour, out float oldG) ||
                        tentativeG < oldG)
                    {
                        cameFrom[neighbour] = current;

                        gScore[neighbour] = tentativeG;

                        fScore[neighbour] =
                            tentativeG +
                            Heuristic(neighbour, targetNode);

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }

            // Пути не существует.
            return null;
        }

        private static float Heuristic(
            NavNode a,
            NavNode b)
        {
            return Vector2.Distance(a.Position, b.Position);
        }

        private static NavNode GetLowestFScore(
            List<NavNode> openSet,
            Dictionary<NavNode, float> fScore)
        {
            NavNode best = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                NavNode node = openSet[i];

                if (fScore[node] < fScore[best])
                    best = node;
            }

            return best;
        }

        private static List<NavNode> ReconstructPath(
            Dictionary<NavNode, NavNode> cameFrom,
            NavNode current)
        {
            var path = new List<NavNode>();

            path.Add(current);

            while (cameFrom.TryGetValue(current, out NavNode parent))
            {
                current = parent;
                path.Add(current);
            }

            path.Reverse();

            return path;
        }
    }
}