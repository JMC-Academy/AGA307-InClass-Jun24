using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pausePanel;
    private bool paused;

    void Start()
    {
        paused = false;
        pausePanel.SetActive(paused);
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        paused = !paused;
        pausePanel.SetActive(paused);
        Time.timeScale = paused ? 0.0f : 1.0f;
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
