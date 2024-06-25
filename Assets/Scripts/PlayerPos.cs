using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class PlayerPos : MonoBehaviour
{
    private GameSession gs;

    private void Start()
    {
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
}
