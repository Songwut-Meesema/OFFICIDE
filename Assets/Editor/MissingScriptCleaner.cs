using UnityEngine;
using UnityEditor;

public class MissingScriptCleaner : EditorWindow
{
    [MenuItem("Tools/Advanced Clean Missing Scripts")]
    public static void CleanMissingScripts()
    {
        int totalCount = 0;
        GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject go in allGameObjects)
        {
            // ข้ามพวก Prefab Assets ที่ไม่ได้อยู่ใน Scene
            if (EditorUtility.IsPersistent(go)) continue;

            // ลบ missing scripts
            int count = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
            if (count > 0)
            {
                Debug.Log($"Removed {count} missing script(s) from GameObject: {go.name}", go);
                totalCount += count;
                EditorUtility.SetDirty(go); // Mark object as dirty
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"✅ Finished! Removed total {totalCount} missing script(s).");
    }
}
