using Entities.Navigation;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Cells
{
    public partial class Cell
    {
        public NavTree NavTree;

        public bool AbleToMove(Vector2 a, Vector2 b) {
            return AbleToMove(a, b, out Vector2 _, out Vector2 _);
        }
        public bool AbleToMove(Vector2 a, Vector2 b, out Vector2 hit, out Vector2 normal)
        {
            hit = Vector2.zero;
            normal = Vector2.zero;
            Vector2 direction = b - a;

            int cellX = Mathf.FloorToInt(a.x);
            int cellY = Mathf.FloorToInt(a.y);

            if (!IsTileEmpty(new Vector2Int(cellX, cellY))) {
                hit = a;
                return false;
                
            }

            if (direction == Vector2.zero) {
                return true;
            }

            int xDirection = direction.x > 0f ? 1 : direction.x < 0f ? -1 : 0;
            int yDirection = direction.y > 0f ? 1 : direction.y < 0f ? -1 : 0;

            float timePerXCell = direction.x != 0f
                ? Mathf.Abs(1f / direction.x)
                : float.PositiveInfinity;

            float timePerYCell = direction.y != 0f
                ? Mathf.Abs(1f / direction.y)
                : float.PositiveInfinity;

            float nextXBoundary = direction.x > 0f
                ? cellX + 1
                : cellX;

            float nextYBoundary = direction.y > 0f
                ? cellY + 1
                : cellY;

            float timeToNextXBoundary = direction.x != 0f
                ? (nextXBoundary - a.x) / direction.x
                : float.PositiveInfinity;

            float timeToNextYBoundary = direction.y != 0f
                ? (nextYBoundary - a.y) / direction.y
                : float.PositiveInfinity;

            while (true)
            {
                float timeToNextBoundary =
                    Mathf.Min(timeToNextXBoundary, timeToNextYBoundary);

                if (timeToNextBoundary > 1f)
                    break;

                Vector2 norm;
                if (timeToNextXBoundary < timeToNextYBoundary)
                {
                    cellX += xDirection;
                    timeToNextXBoundary += timePerXCell;
                    norm = new Vector2(-xDirection, 0);
                }
                else if (timeToNextYBoundary < timeToNextXBoundary)
                {
                    cellY += yDirection;
                    timeToNextYBoundary += timePerYCell;
                    norm = new Vector2(0, -yDirection);
                }
                else
                {
                    cellX += xDirection;
                    cellY += yDirection;

                    timeToNextXBoundary += timePerXCell;
                    timeToNextYBoundary += timePerYCell;
                    norm = new Vector2(-xDirection, -yDirection).normalized;
                }

                if (!IsTileEmpty(new Vector2Int(cellX, cellY))) {
                    hit = a + direction * timeToNextBoundary;
                    normal = norm;
                    return false;
                }
            }
            return true;
        }
    }
}