using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelData[] levels;
    private LevelHandler currentLevel;

    private Dictionary<Vector2, LevelData> loadedLevels;

    private Vector2 currentLevelIndex = Vector2.zero;
    public Vector2 CurrentLevelIndex { get { return currentLevelIndex;} }

    [Header("Important Level Objects")]
    [SerializeField]
    private PlayerController playerController;
    public PlayerController PlayerController { get { return playerController;} }
    [SerializeField] private CameraController cameraController;
    public CameraController CameraController { get { return cameraController;} }


    private void Start()
    {
        loadedLevels = new Dictionary<Vector2, LevelData>();
    }

    public void SpawnInNextLevel(Vector2 newLevelIndex)
    {
        currentLevelIndex = newLevelIndex;

        if (loadedLevels.ContainsKey(currentLevelIndex))
            return;
        Debug.Log(newLevelIndex);
        LevelData newLevel = null;

        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].levelIndex == newLevelIndex)
            {
                newLevel = levels[i];
                break;
            }
        }

        if(newLevel == null)
            Debug.LogError("NO LEVEL FOUND TO LOAD");

        GameObject levelGeo = Instantiate(newLevel.levelGeometry,newLevel.levelSpawnPoint,Quaternion.identity,this.transform);
        loadedLevels.Add(currentLevelIndex,newLevel);
    }
}
