using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public static GameManager Instance
    {
        get {return _instance;}
    }

    private GameObject deathScreen;
    public GameObject pauseMenuCanvas;
    private bool isGamePaused = false;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        InitGame();
    }

    private void Start()
    {
        deathScreen = transform.Find("DeathScreen").gameObject;
        pauseMenuCanvas = transform.Find("PauseMenuCanvas").gameObject;
        pauseMenuCanvas.SetActive(false); 
    }

    void InitGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PlayerDeath()
    {
        deathScreen.SetActive(true);
        GameObject player = GameObject.FindGameObjectWithTag("Player").gameObject;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerHealth>().enabled = false;
        foreach (Transform child in player.transform)
        {
            if (child.tag != "MainCamera")
                child.gameObject.SetActive(false);
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        player.tag = "Untagged";
    }

    void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; 
        pauseMenuCanvas.SetActive(true); 
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        pauseMenuCanvas.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
        #else
        Application.Quit(); 
        #endif
    }
}
