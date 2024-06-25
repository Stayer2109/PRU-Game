using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DestroySession : MonoBehaviour
{
    // Start is called before the first frame update
    private DestroySession session;

    void Awake()
    {
        session = FindObjectOfType<DestroySession>();
    }

    void Start()
    {
        Destroy(session);
    }
}
