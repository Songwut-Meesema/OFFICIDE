using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;   
    public GameObject player;        
    
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);    
        Time.timeScale = 1f;            
        isPaused = false;               
        player.GetComponent<PlayerMovement>().enabled = true; 
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);     
        Time.timeScale = 0f;             
        isPaused = true;                
        player.GetComponent<PlayerMovement>().enabled = false; 
    }

    public void Restart()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resume(); 
    }

    public void Exit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}