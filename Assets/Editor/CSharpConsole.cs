using UnityEditor;
using UnityEngine;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Threading.Tasks;

public class CSharpConsoleWindow : EditorWindow
{
    string input = "";
    Vector2 scroll;
    string output = "";

    ScriptState<object> state;

    [MenuItem("Tools/C# Console")]
    static void Open()
    {
        GetWindow<CSharpConsoleWindow>("C# Console");
    }

    void OnGUI()
    {
        GUILayout.Label("C# REPL (Editor)", EditorStyles.boldLabel);

        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(150));
        GUILayout.Label(output, EditorStyles.wordWrappedLabel);
        EditorGUILayout.EndScrollView();

        GUI.SetNextControlName("Input");
        input = EditorGUILayout.TextArea(input, GUILayout.Height(80));

        if (GUILayout.Button("Execute") || 
            (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return))
        {
            Execute();
            GUI.FocusControl("Input");
        }
    }

    async void Execute()
    {
        try
        {
            var options = ScriptOptions.Default
                .AddReferences(
                    typeof(GameObject).Assembly,
                    typeof(Selection).Assembly
                )
                .AddImports(
                    "System",
                    "UnityEngine",
                    "UnityEditor"
                );
            
            var modInput = "var @this = Selection.activeGameObject; private static T @comp<T>() { return @this.GetComponent<T>(); }" + input;
            if (state == null)
                state = await CSharpScript.RunAsync(modInput, options);
            else
                state = await state.ContinueWithAsync(modInput);

            if (state.ReturnValue != null)
                output += $"> {state.ReturnValue}\n";
        }
        catch (System.Exception e)
        {
            output += $"ERROR: {e.Message}\n";
        }

        input = "";
        Repaint();
    }

   
}