using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class BrickLayer : EditorWindow

{
    public static BrickLayer thisWindow;
    // MenuItem lets us run a static function from the menu bar
                            //menuItem is the path (e.g. File/Save)
    [MenuItem("Brick Layer", menuItem = "MyGame/Brick Layer")]
    public static void Open()
    {
        // get or open a new window
        thisWindow = GetWindow<BrickLayer>("Brick Layer");
    }
    
    // this runs every frame the window is currently active (last thing selected)

    public int rows, columns;
    public GameObject prefab;
    
    public Transform parent;

    public List<GameObject> bricksInScene = new();

    private void OnGUI()
    {
        // (GameObject) means cast our result into the GameObject type
        // Other objects include sprites, components, any other unity object that isn't a GameObject
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab to spawn", prefab, typeof(GameObject), false);

        parent = (Transform)EditorGUILayout.ObjectField("Parent GameObject", parent, typeof(Transform), true);
        //
        // Set rows to whatever value is put in the field (which will b rows if no new number has been added
        rows = Mathf.Max(EditorGUILayout.IntField("Rows", rows), 0);
        columns = Mathf.Max(EditorGUILayout.IntField("Columns", columns), 0);
        
        EditorGUI.BeginDisabledGroup((prefab == null));
        // in one go, draw the button, and also check if it's been clicked
        if (GUILayout.Button("Lay bricks"))
        {
            SpawnObjects();
        }
        EditorGUI.EndDisabledGroup();

        // only draw the button if there are bricks to be cleared
        if (bricksInScene.Count > 0)
        {
            if (GUILayout.Button("Clear bricks"))
            {
                ClearObjects();
            }
        }
    }

    private void SpawnObjects()
    {
        float gap = 0.1f; 
        // get the prefab's x scale, add our gap
        float brickWidth = prefab.transform.localScale.x;
        // Get the brick height as well as the gap to leave
        float brickHeightPlusGap = prefab.transform.localScale.y + gap;
        // get the prefab's x scale, add our gap, and multiply by the number of columns, to figure out our start point
        float halfWidth = -((brickWidth * columns + gap * columns + gap * columns-1) -brickWidth) / 2f;

        int count = 0;
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                count++;
                GameObject spawned = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                spawned.transform.position = new Vector3(halfWidth + (brickWidth + gap) * i, //.x
                                                        4.5f - brickHeightPlusGap * j); //.y
                spawned.name += " (" + count + ")";

                if (parent)
                {
                    spawned.transform.parent = parent;
                }
                
                bricksInScene.Add(spawned);
            }
        }
        MarkSceneDirty();
    }

    private void ClearObjects()
    {
        for (int i = 0; i < bricksInScene.Count; i++)
        {
            GameObject current = bricksInScene[0];

            if (current == null || !current.scene.IsValid()) continue;
            
            // remove the object from the list
            bricksInScene.Remove(current);
            // destroy the object
            DestroyImmediate(current);
        }
        MarkSceneDirty();
    }

    private void MarkSceneDirty()
    {
        // Get the currently active scene and mark it dirty
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}

