using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private Vector2 preTapJumpPos = new Vector2(22, 0);
    private Vector2 preFullJumpPos = new Vector2(48, 0);

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void TapJumpPosReset()
    {
        player.transform.position = preTapJumpPos;
    }

    public void FullJumpPosReset()
    {
        player.transform.position = preFullJumpPos;
    }
}
