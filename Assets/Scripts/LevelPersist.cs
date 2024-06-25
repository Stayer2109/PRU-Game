using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPersist : MonoBehaviour
{
    private static LevelPersist instance;
    void Awake()
    {
        // int levelPersistsCount = FindObjectsOfType<LevelPersist>().Length;

        // if (levelPersistsCount > 1)
        //     Destroy(gameObject);
        // else
        //     DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetLevelPersist()
    {
        Destroy(gameObject);
    }
}
