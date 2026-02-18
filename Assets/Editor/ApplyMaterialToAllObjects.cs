using UnityEngine;
using UnityEditor;

public class ApplyMaterialToAllObjects : EditorWindow
{
    Material materialToApply;

    [MenuItem("Tools/Apply Material To All Renderers")]
    public static void ShowWindow()
    {
        GetWindow<ApplyMaterialToAllObjects>("Apply Material");
    }

    void OnGUI()
    {
        GUILayout.Label("Apply Material To All Objects", EditorStyles.boldLabel);
        materialToApply = (Material)EditorGUILayout.ObjectField("Material", materialToApply, typeof(Material), false);

        if (GUILayout.Button("Apply Material"))
        {
            if (materialToApply != null)
            {
                ApplyMaterial();
            }
            else
            {
                Debug.LogWarning("Please assign a material first.");
            }
        }
    }

    void ApplyMaterial()
    {
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        int count = 0;

        foreach (Renderer rend in renderers)
        {
            Undo.RecordObject(rend, "Apply Material");
            rend.sharedMaterial = materialToApply;
            count++;
        }

        Debug.Log($"Applied material to {count} renderers in the scene.");
    }
}
