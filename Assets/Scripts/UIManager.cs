using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private GameObject canvasGO;

    // Start is called before the first frame update
    void Start()
    {
        canvasGO = GameObject.Find("Canvas");
    }

    public void ShowHideUI(bool isActive)
    {
        canvasGO.SetActive(isActive);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
