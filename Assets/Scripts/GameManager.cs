using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SpawnManager spawnManager;
    void Start()
    {
        spawnManager = GameObject.FindObjectOfType<SpawnManager>();

        spawnManager.SpawnPlayer();
        spawnManager.StartSpawningEnemyShips();
    }

    
}
