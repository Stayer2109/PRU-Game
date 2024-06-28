using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayerPos : MonoBehaviour
{
    private TutorialController tController;
    private GameSession gs;

    private void Start()
    {
        tController = FindObjectOfType<TutorialController>();
        gs = GameObject.FindGameObjectWithTag("GameSession").GetComponent<GameSession>();
        ResetPos();
    }

    public void ResetPos()
    {
        if (gs != null)
        {
            if (gs.lastCheckpointPos == Vector2.zero)
            {
                return;
            }

            transform.position = gs.lastCheckpointPos;
        }
    }

    public void SetPos(Vector2 pos)
    {
        gs = FindObjectOfType<GameSession>();
        if (gs != null)
        {
            gs.lastCheckpointPos = pos;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // after colliding, waiting for 1s to reset the player position
        if (other.gameObject.CompareTag("ResetPlatform"))
        {
            Invoke("ResetPosPreTapJump", 0.5f);
        }
        else if (other.gameObject.CompareTag("ResetFullPlatform"))
        {
            Invoke("ResetPosPreFullJump", 0.5f);
        }
    }

    private void ResetPosPreTapJump()
    {
        tController.TapJumpPosReset();
    }

    private void ResetPosPreFullJump()
    {
        tController.FullJumpPosReset();
    }
}
