using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool GameIsPaused = false;
    private bool HideMouse = true;
    public GameObject PauseMenuVisuals;

    void Start()
    {
        PauseMenuVisuals.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused == true)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        PauseMenuVisuals.SetActive(false);
        PauseMenuVisuals.SetActive(false);
        Cursor.visible = !HideMouse;
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    void PauseGame()
    {
        PauseMenuVisuals.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }

    public void ToMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}
