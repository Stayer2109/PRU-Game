using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainCanvas; // Reference to the main canvas
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject chooseLevelButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject levelSelectionPanel; // Reference to the level selection canvas
    [SerializeField] private GameObject levelButtonPrefab; // Prefab for level buttons
    [SerializeField] private Transform levelButtonParent; // Parent transform for level buttons
    [SerializeField] private GameObject closeButton;

    private void Start()
    {
        mainCanvas.SetActive(true); // Ensure main canvas is visible
        levelSelectionPanel.SetActive(false); // Ensure level selection canvas is hidden initially

        // Add listeners to buttons
        playButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGame);
        chooseLevelButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ShowLevelSelection);
        exitButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ExitGame);
        closeButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CloseLevelSelection);
    }

    public void StartGame()
    {
        Debug.Log("Play button clicked");
        mainCanvas.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Debug.Log("Exit button clicked");
        Application.Quit();
    }

    public void ShowLevelSelection()
    {
        Debug.Log("Choose Level button clicked");
        levelSelectionPanel.SetActive(true);
    }

    public void CloseLevelSelection()
    {
        levelSelectionPanel.SetActive(false);
        mainCanvas.SetActive(true);
    }

    public void LoadLevel(int index)
    {
        Debug.Log("Loading level: " + index);
        SceneManager.LoadScene(index);
    }
}
