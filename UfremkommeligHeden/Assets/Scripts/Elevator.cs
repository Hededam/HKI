using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] 
    private Vector3 targetPosition = default;
    private bool touchingPlayer = false;
    private void Start()
    {
        targetPosition = new Vector3(
            transform.position.x,
            transform.position.y + 10f,
            transform.position.z);
    }
    
    private void FixedUpdate()
    {
        if (touchingPlayer)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                4f * Time.fixedDeltaTime);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            touchingPlayer = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            touchingPlayer = false;
    }
}
