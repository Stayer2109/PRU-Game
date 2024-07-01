using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField]
    public int playerLives = 8;

    [SerializeField]
    int playerScore = 0;

    [SerializeField]
    int level = 0;

    [SerializeField]
    TextMeshProUGUI livesText;

    [SerializeField]
    TextMeshProUGUI scoreText;

    [SerializeField]
    TextMeshProUGUI levelText;

    [SerializeField]
    GameObject dashNotification;

    [SerializeField]
    GameObject statsCanvas;

    [SerializeField]
    GameObject menuCanvas;

    [SerializeField]
    GameObject gameoverCanvas;

    [SerializeField]
    GameObject playButton;

    [SerializeField]
    GameObject restartButton;

    [SerializeField]
    GameObject exitButton;

    public Vector2 lastCheckpointPos;
    public PlayerPos playerPos;
    public bool isPaused = true;
    private static GameSession instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        isPaused = false;
    }

    void Start()
    {
        checkIsTutorial();
        UpdateLivesText();
        UpdateScoreText();
        SetLevel(level);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // disable after 1s
        if (dashNotification != null)
        {
            StartCoroutine(DisableDashNotificationAfterDelay());
        }
    }
    IEnumerator DisableDashNotificationAfterDelay()
    {
        yield return new WaitForSeconds(5f); // Wait for 1 second

        // Disable dashNotification
        dashNotification.SetActive(false);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 1)  // Assuming buildIndex 1 is the tutorial
        {
            statsCanvas.SetActive(true);
        }
        else
        {
            statsCanvas.SetActive(false);
        }

        ResetPlayerPos();
        UpdateLivesText();
        UpdateScoreText();
        SetLevel(scene.buildIndex);
    }

    private void UpdateLevelText()
    {
        levelText.text = "Level : " + level;
    }

    public void SetLevel(int levelIndex)
    {
        level = level == 0 ? SceneManager.GetActiveScene().buildIndex : levelIndex - 1;
        UpdateLevelText();
    }

    void OnExit()
    {
        if (!isPaused)
        {
            ShowMenu();
        }
        else
        {
            CloseMenu();
        }
    }

    public void OnPlayButtonClick()
    {
        CloseMenu();
    }

    public void OnExitButtonClick()
    {
        ResetSession();
    }

    public void OnRestartButtonClick()
    {
        RestartGame();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ShowGameOver();
        }
    }

    public void ProcessPlayerScore()
    {
        playerScore += 50;
        UpdateScoreText();
    }

    public void ResetPlayerLives()
    {
        playerLives = 8;
        UpdateLivesText();
    }

    void TakeLife()
    {
        playerLives--;
        ReloadCurrentLevel();
        UpdateLivesText();
    }

    void ReloadCurrentLevel()
    {
        ResetPlayerPos();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetSession()
    {
        FindObjectOfType<LevelPersist>().ResetLevelPersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    void UpdateLivesText()
    {
        livesText.text = "Lives : " + playerLives;
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score : " + playerScore;
    }

    void ShowMenu()
    {
        isPaused = true;
        statsCanvas.SetActive(false);
        menuCanvas.SetActive(true);
    }

    void CloseMenu()
    {
        isPaused = false;
        if (!IsTutorial())
            statsCanvas.SetActive(true);
        menuCanvas.SetActive(false);
    }

    void ResetPlayerPos()
    {
        if (playerPos == null)
        {
            playerPos = FindObjectOfType<PlayerPos>();
        }
    }

    public void ShowGameOver()
    {
        isPaused = true;
        statsCanvas.SetActive(false);
        gameoverCanvas.SetActive(true);
    }

    public void RestartGame()
    {
        FindObjectOfType<LevelPersist>().ResetLevelPersist();
        Destroy(gameObject);
        ResetPlayerLives();
        isPaused = false;
        statsCanvas.SetActive(true);
        gameoverCanvas.SetActive(false);
        ReloadCurrentLevel();
    }

    bool IsTutorial()
    {
        return SceneManager.GetActiveScene().buildIndex == 1;
    }

    public void checkIsTutorial()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        if (IsTutorial())
        {
            statsCanvas.SetActive(false);
        }
        else
        {
            statsCanvas.SetActive(true);
        }
    }
}
