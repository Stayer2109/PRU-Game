using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCanvas; // Reference to the main canvas

    [SerializeField]
    private GameObject playButton;

    [SerializeField]
    private GameObject chooseLevelButton;

    [SerializeField]
    private GameObject exitButton;

    [SerializeField]
    private GameObject levelSelectionPanel; // Reference to the level selection canvas

    [SerializeField]
    private GameObject levelButtonPrefab; // Prefab for level buttons

    [SerializeField]
    private Transform levelButtonParent; // Parent transform for level buttons

    [SerializeField]
    private GameObject closeButton;

    private GameSession gs;

    private void Start()
    {
        mainCanvas.SetActive(true); // Ensure main canvas is visible
        levelSelectionPanel.SetActive(false); // Ensure level selection canvas is hidden initially
        gs = FindAnyObjectByType<GameSession>();

        // Add listeners to buttons
        playButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGame);
        chooseLevelButton
            .GetComponent<UnityEngine.UI.Button>()
            .onClick.AddListener(ShowLevelSelection);
        exitButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ExitGame);
        closeButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CloseLevelSelection);
    }

    public void StartGame()
    {
        mainCanvas.SetActive(false);
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        SceneManager.LoadScene(nextLevelIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowLevelSelection()
    {
        levelSelectionPanel.SetActive(true);
    }

    public void CloseLevelSelection()
    {
        levelSelectionPanel.SetActive(false);
        mainCanvas.SetActive(true);
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
}
