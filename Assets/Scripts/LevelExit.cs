using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField]
    float levelLoadDelay = 0.25f;

    void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex + 1;

        if (nextLevelIndex == SceneManager.sceneCountInBuildSettings)
            nextLevelIndex = 0;

        if (nextLevelIndex == 5)
            FindAnyObjectByType<GameSession>().ResetSession();

        FindAnyObjectByType<LevelPersist>().ResetLevelPersist();
        FindObjectOfType<GameSession>().SetLevel(nextLevelIndex);
        FindObjectOfType<GameSession>().ResetPlayerLives();
        FindObjectOfType<PlayerPos>().SetPos(new Vector2(0, 0));
        SceneManager.LoadScene(nextLevelIndex);
    }
}
