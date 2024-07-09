using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    float holdDuration = 1.2f;
    private Slider slider;
    private float holdTimer = 0;
    bool isHolding = false;

    void Start()
    {
        slider = FindObjectOfType<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            GameSession gs = FindObjectOfType<GameSession>();
            if (!slider && gs.IsTutorial())
                return;

            // set slider opacity
            slider.gameObject.SetActive(true);

            holdTimer += Time.deltaTime;
            slider.value = holdTimer / holdDuration;
            
            if (holdTimer >= holdDuration)
            {
                // get session and load next level
                int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
                int nextLevelIndex = currentLevelIndex + 1;
                gs.SetLevel(nextLevelIndex);
                gs.ResetPlayerLives();
                gs.ResetSession();
                SceneManager.LoadScene(nextLevelIndex);
            }
        }
    }

    public void OnHold(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isHolding = true;
        }
        else if (context.canceled)
        {
            // check if slider is null
            if (!slider)
                return;

            slider.gameObject.SetActive(false);
            ResetHold();
        }
    }

    private void ResetHold()
    {
        isHolding = false;
        holdTimer = 0;
    }
}
