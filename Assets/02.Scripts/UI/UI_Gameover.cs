using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Gameover : MonoBehaviour
{

    public static UI_Gameover Instance {  get; private set; }
   // public image 
    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }

    }
    private void OnEnable()
    {
        
    }

    public void OnClickRetryButton()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    public void OnClickQuitButton()
    {
        Application.Quit();

    }

}
