using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CreateAssetMenu(fileName = "Bell", menuName = "Data/Bell",order = 1)]
#endif
public class Bell : ScriptableObject
{
    public GameObject soundwavePrefab;
}
