using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class PlayerPos : MonoBehaviour
{
    private GameSession gs;

    // Start is called before the first frame update
    void Start()
    {
        gs = FindObjectOfType<GameSession>();
        ResetPos();
    }

    public void ResetPos()
    {
        if (gs != null)
        {
            if (gs.lastCheckpointPos == Vector2.zero)
                return;

            transform.position = gs.lastCheckpointPos;
        }
    }
}
