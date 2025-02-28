using System;
using UnityEditor;
using UnityEngine;

public class PlaceObject : EditorWindow
{
    public static PlaceObject thisWindow;
    [MenuItem("Place Object", menuItem = ("MyGame/Place Object"))]
    public static void Open()
    {
        thisWindow = GetWindow<PlaceObject>("Place Object");
    }

    public GameObject prefab;

    public Vector3 scenePosition;

    private void OnGUI()
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Object to spawn", prefab, typeof(GameObject), true);
        scenePosition = EditorGUILayout.Vector3Field("Position", scenePosition);

        EditorGUI.BeginDisabledGroup(prefab == null);
        if (GUILayout.Button("Spawn Object"))
        {
            Instantiate(prefab, scenePosition, Quaternion.identity);
        }
        EditorGUI.EndDisabledGroup();
    }
}
