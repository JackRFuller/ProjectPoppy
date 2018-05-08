using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    [SerializeField]
    private Transform[] levelSpawnPoints;

    public void SetPlayerToSpawnPoint(PlayerMovementController playerMovementController, int spawnIndex)
    {
        if (playerMovementController == null)
            playerMovementController = this.GetComponent<PlayerMovementController>();

        playerMovementController.SetPlayerPosition(levelSpawnPoints[spawnIndex]);        

        playerMovementController.gameObject.SetActive(true);
    }
	
}
