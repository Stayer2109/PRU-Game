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
    GameObject statsCanvas;

    [SerializeField]
    GameObject menuCanvas;

    [SerializeField]
    GameObject playButton;

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
        UpdateLivesText();
        UpdateScoreText();
        SetLevel(level);
    }

    private void UpdateLevelText()
    {
        levelText.text = "Level : " + level;
    }

    public void SetLevel(int levelIndex)
    {
        level = level == 0 ? SceneManager.GetActiveScene().buildIndex : levelIndex;
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

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetSession();
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
}
