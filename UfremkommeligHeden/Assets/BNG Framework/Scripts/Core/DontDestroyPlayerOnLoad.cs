using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyPlayerOnLoad : MonoBehaviour
{
    public static DontDestroyPlayerOnLoad Instance;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
