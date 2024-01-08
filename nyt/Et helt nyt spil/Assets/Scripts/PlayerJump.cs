using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{

    public float jumpForce = 1000f;

    Rigidbody player_Rigidbody;
    


    void Awake()
    {

        player_Rigidbody = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        if (Input.GetButtonUp("Jump")) 
        {
           Debug.Log("jump was pressed");

            //Apply a force to this Rigidbody in direction of this GameObjects up axis
           player_Rigidbody.AddForce(transform.up * jumpForce);
        }
    }
}
