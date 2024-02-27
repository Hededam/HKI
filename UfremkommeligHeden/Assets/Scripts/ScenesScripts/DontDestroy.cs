using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    //commented out sections are an example of how to create a singlton object, destroys the duplicate object on return. 
    
    //private string objTag;

    void Start()
    {
       // objTag = tag;

        StartCoroutine(KeepMe());
    }

    private IEnumerator KeepMe()

    {
        yield return new WaitForSeconds(1);

       // GameObject[] objs = GameObject.FindGameObjectsWithTag(objTag);

       // if (objs.Length > 1)
       // {
          //  Destroy(gameObject);
        //}

        DontDestroyOnLoad(gameObject);
    }
}
