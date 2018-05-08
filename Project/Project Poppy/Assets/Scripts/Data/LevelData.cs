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
    public Vector3 levelSpawnPoint;
    public Vector3 levelRotation; //Determines what rotation the camera should be at
}
