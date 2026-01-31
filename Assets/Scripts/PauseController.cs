using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;

    [Tooltip("אם false, לחיצה על Esc לא תפעיל Pause (למשל בתפריט הראשי)")]
    public bool canPause = true;

    private bool isPaused;

    private void Start()
    {
        SetPaused(false);
    }

    private void Update()
    {
        if (!canPause)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            SetPaused(!isPaused);
    }

    public void Continue() => SetPaused(false);

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetPaused(bool value)
    {
        if (!canPause && value)
            return;

        isPaused = value;
        if (pauseCanvas) pauseCanvas.SetActive(value);
        Time.timeScale = value ? 0f : 1f;
    }
}
