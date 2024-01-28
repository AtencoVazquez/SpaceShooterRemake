using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject playerShip, enemyShip;

    private const float enemyPositionOffset = 0.75f;
    private float screenBoundLeft, screenBoundRight, screenBoundUp;
    private bool _stopEnemySpawning;
    [SerializeField] private float enemySpawnTime;

    public bool StopEnemySpawning { get => _stopEnemySpawning; set => _stopEnemySpawning = value; }

    private void Awake()
    {
        screenBoundLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        screenBoundRight = Camera.main.orthographicSize * Camera.main.aspect;
        screenBoundUp = Camera.main.orthographicSize;
    }

    void Start()
    {
        
    }
    public void SpawnPlayer()
    {
        Instantiate(playerShip, new Vector2(0,0), Quaternion.identity);
    }

    public void StartSpawningEnemyShips()
    {
        StopEnemySpawning = false;
        StartCoroutine(SpawnEnemyShips());
    }

    public void StopSpawningEnemyShips()
    {
        StopEnemySpawning = true;
        StopCoroutine(SpawnEnemyShips());
    }

    private float GenerateRandomPosition(float minValue, float maxValue)
    {
        float randomPosition;
        randomPosition = Random.Range(minValue, maxValue);
        return randomPosition;
    }

    IEnumerator SpawnEnemyShips()
    {
        float xSpawnPosition;
        float ySpawnPosition;

        while (!StopEnemySpawning) 
        {
            xSpawnPosition = GenerateRandomPosition(screenBoundLeft + enemyPositionOffset, screenBoundRight - enemyPositionOffset);
            ySpawnPosition = screenBoundUp + (enemyPositionOffset * 3);
            Instantiate(enemyShip, new Vector2(xSpawnPosition, ySpawnPosition), Quaternion.identity);
            yield return new WaitForSeconds(enemySpawnTime);
        }
    }

    private void OnApplicationQuit()
    {
        StopEnemySpawning = true;
        StopAllCoroutines();
    }

}
