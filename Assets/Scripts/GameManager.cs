using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private SpawnManager spawnManager;
    private UIManager uiManager;
    private bool _isGameOver;
    private bool _isGamePaused;
    public bool IsGameOver { get => _isGameOver; set => _isGameOver = value; }
    public bool IsGamePaused { get => _isGamePaused; set => _isGamePaused = value; }

    void Start()
    {
        IsGameOver = false;
        spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        uiManager = GameObject.FindObjectOfType<UIManager>();

        spawnManager.SpawnPlayer();
        spawnManager.StartSpawningEnemyShips();
        uiManager.ShowHideUI(false);

        IsGamePaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            IsGamePaused = !IsGamePaused;

            if (IsGamePaused == true)
                Time.timeScale = 0.0f;
            else
                Time.timeScale = 1.0f;
        }


            
    }

    public void GameOver()
    {
        IsGameOver = true;
        spawnManager.StopSpawningEnemyShips();
        uiManager.ShowHideUI(true);
    }

}
