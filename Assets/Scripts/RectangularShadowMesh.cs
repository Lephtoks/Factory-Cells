using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public static class RectangularShadowMesh
{
    /// <summary>
    /// Создаёт меш квадрата [0,1]×[0,1] с вершинами на серединах сторон.
    /// </summary>
    public static Mesh Create()
    {
        var mesh = new Mesh
        {
            name = "RectangularShadowMesh"
        };

        // 0 — нижний левый угол
        // 1 — середина нижней стороны
        // 2 — нижний правый угол
        // 3 — середина правой стороны
        // 4 — верхний правый угол
        // 5 — середина верхней стороны
        // 6 — верхний левый угол
        // 7 — середина левой стороны
        Vector3[] vertices =
        {
            new Vector3(0.0f, 0.0f, 0.0f), // 0
            new Vector3(0.5f, 0.0f, 0.0f), // 1
            new Vector3(1.0f, 0.0f, 0.0f), // 2
            new Vector3(1.0f, 0.5f, 0.0f), // 3
            new Vector3(1.0f, 1.0f, 0.0f), // 4
            new Vector3(0.5f, 1.0f, 0.0f), // 5
            new Vector3(0.0f, 1.0f, 0.0f), // 6
            new Vector3(0.0f, 0.5f, 0.0f)  // 7
        };

        // UV совпадают с позициями (удобно для возможного смягчения краёв)
        Vector2[] uvs =
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.5f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 0.5f),
            new Vector2(1.0f, 1.0f),
            new Vector2(0.5f, 1.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(0.0f, 0.5f)
        };

        // Индексы треугольников (порядок против часовой стрелки)
        // 4 угловых треугольника + 2 центральных, покрывающих ромб из средних точек
        int[] triangles =
        {
            // Нижний левый угол
            0, 1, 7,

            // Нижний правый угол
            1, 2, 3,

            // Верхний правый угол
            3, 4, 5,

            // Верхний левый угол
            5, 6, 7,

            // Центральный ромб (разбит на два треугольника)
            1, 3, 5,
            1, 5, 7
        };

        mesh.vertices  = vertices;
        mesh.uv        = uvs;
        mesh.triangles = triangles;

        // Нормали не обязательны для данного шейдера, но задаём их для полноты
        Vector3[] normals = new Vector3[8];
        for (int i = 0; i < 8; i++)
            normals[i] = Vector3.forward;
        mesh.normals = normals;

        mesh.RecalculateBounds();
        mesh.RecalculateTangents(); // на случай, если позже понадобятся

        return mesh;
    }
}
#if UNITY_EDITOR
public static class RectangularShadowMeshCreator
{
    [MenuItem("Assets/Create/Rectangular Shadow Mesh")]
    public static void CreateAsset()
    {
        Mesh mesh = RectangularShadowMesh.Create();

        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/RectangularShadowMesh.asset");
        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = mesh;
    }
}
#endif