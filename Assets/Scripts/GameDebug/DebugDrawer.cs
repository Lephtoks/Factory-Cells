using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace GameDebug
{
    public static class DebugDrawer
    {
        private static readonly List<DebugLine> lines = new();

        public static void OnPostRender(Camera camera)
        {
            if (lines.Count == 0)
                return;
            AssetProvider.Instance.registry.render.DebugLineMaterial.SetPass(0);

            GL.PushMatrix();

            GL.modelview = camera.worldToCameraMatrix;
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            GL.Begin(GL.LINES);

            foreach (var line in lines)
            {
                GL.Color(line.color);

                GL.Vertex(line.from);
                GL.Vertex(line.to);
            }

            GL.End();

            GL.PopMatrix();

            lines.Clear();
        }

        public static void Line(Vector3 from, Vector3 to, Color color)
        {
            lines.Add(new DebugLine
            {
                from = from,
                to = to,
                color = color
            });
        }

        public static void Ray(Vector3 origin, Vector3 direction, Color color)
        {
            Line(origin, origin + direction, color);
        }

        public static void Rect(Rect rect, Color color)
        {
            Rect(rect.center, rect.size, color);
        }

        public static void Rect(Vector2 center, Vector2 size, Color color)
        {
            Rect(center, size, 0f, color);
        }

        public static void Rect(Vector2 center, Vector2 size, float rotation, Color color)
        {
            Vector2 half = size * 0.5f;

            Quaternion rot = Quaternion.Euler(0, 0, rotation);

            Vector3 a = center + (Vector2)(rot * new Vector2(-half.x, half.y));
            Vector3 b = center + (Vector2)(rot * new Vector2( half.x, half.y));
            Vector3 c = center + (Vector2)(rot * new Vector2( half.x,-half.y));
            Vector3 d = center + (Vector2)(rot * new Vector2(-half.x,-half.y));

            Line(a, b, color);
            Line(b, c, color);
            Line(c, d, color);
            Line(d, a, color);
        }

        public static void Bounds(Bounds bounds, Color color)
        {
            Rect(bounds.center, bounds.size, color);
        }

        public static void Circle(Vector2 center, float radius, Color color, int segments = 32)
        {
            float step = Mathf.PI * 2f / segments;

            Vector3 previous = center + new Vector2(radius, 0);

            for (int i = 1; i <= segments; i++)
            {
                float angle = i * step;

                Vector3 current = center + new Vector2(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius);

                Line(previous, current, color);

                previous = current;
            }
        }

        public static void Polygon(IReadOnlyList<Vector2> points, Color color)
        {
            if (points == null || points.Count < 2)
                return;

            for (int i = 0; i < points.Count; i++)
            {
                Line(
                    points[i],
                    points[(i + 1) % points.Count],
                    color);
            }
        }

        public static void Cross(Vector2 center, float size, Color color)
        {
            float h = size * 0.5f;

            Line(
                center + Vector2.left * h,
                center + Vector2.right * h,
                color);

            Line(
                center + Vector2.up * h,
                center + Vector2.down * h,
                color);
        }

        public static void Arrow(
            Vector2 from,
            Vector2 to,
            Color color,
            float headLength = 0.2f,
            float headAngle = 25f)
        {
            Line(from, to, color);

            Vector2 dir = (from - to).normalized;

            Quaternion left = Quaternion.Euler(0, 0, headAngle);
            Quaternion right = Quaternion.Euler(0, 0, -headAngle);

            Line(to, to + (Vector2)(left * dir) * headLength, color);
            Line(to, to + (Vector2)(right * dir) * headLength, color);
        }

        public static void Point(Vector2 position, Color color, float size = 0.1f)
        {
            Cross(position, size, color);
        }
    }

    internal struct DebugLine
    {
        public Vector3 from;
        public Vector3 to;
        public Color color;
    }
}