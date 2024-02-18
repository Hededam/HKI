using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//det her script gør at det objekt det sidder på ikke blever slettet ved sene skift
public class KeepInventory : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}