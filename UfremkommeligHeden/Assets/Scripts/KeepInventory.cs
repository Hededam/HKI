using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//det her script g�r at det objekt det sidder p� ikke blever slettet ved sene skift
public class KeepInventory : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}