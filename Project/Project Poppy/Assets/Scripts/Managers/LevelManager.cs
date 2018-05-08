using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private LevelData[] levels;
    private List<int> loadedLevels;

    //Camera Attributes
    private CameraController cameraController;

    private LevelHandler currentLevel;
    private int playerSpawnPointInLevel;

    private void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    //Checks if level has been loaded in and if not loads in level
    public void StartMovementToNextLevel(int levelID, int spawnPointID)
    {
        playerSpawnPointInLevel = spawnPointID;

        if (loadedLevels == null)
            loadedLevels = new List<int>();

        LevelData level = levels[levelID - 1];        

        if (!loadedLevels.Contains(levelID))
        {
            GameObject levelGeometry = level.levelGeometry;
            GameObject newLevel = Instantiate(levelGeometry, level.levelSpawnPoint, levelGeometry.transform.rotation, this.transform.root);
            loadedLevels.Add(levelID);
        }

        currentLevel = level.levelGeometry.GetComponent<LevelHandler>();

        StartMovingCameraToNextLevel(level);
    }

    public void StartMovingCameraToNextLevel(LevelData targetLevel)
    {
        cameraController.MoveToPosition(targetLevel.levelSpawnPoint, targetLevel.levelRotation);
    }

    public void SpawnPlayerAtNewPosition()
    {
        StartCoroutine(WaitToRevealPlayer());
    }

    IEnumerator WaitToRevealPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        currentLevel.SetPlayerToSpawnPoint(player.GetComponent<PlayerMovementController>(), playerSpawnPointInLevel);        
    }
	
}
