using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameSession gs;

    // Start is called before the first frame update
    private void Awake()
    {
        gs = GameObject.FindGameObjectWithTag("GameSession").GetComponent<GameSession>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gs.lastCheckpointPos = transform.position;
            Destroy(gameObject);
        }
    }
}
