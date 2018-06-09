using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CreateAssetMenu(fileName = "Level",menuName = "Data/Level", order = 1)]
#endif
public class LevelData : ScriptableObject
{    
    public GameObject levelGeometry;
    public Vector2 levelIndex; //Defines the grid position of the level
    [Space]
    public Vector2 levelSpawnPoint;
    public Vector2 levelRotation; //Determines what rotation the camera should be at
}
