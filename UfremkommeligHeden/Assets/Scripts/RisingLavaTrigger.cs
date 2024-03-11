using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingLavaTrigger : MonoBehaviour
{
    [SerializeField]
    private Vector3 targetPosition = default;

    private bool touchingPlayer = false;
    public GameObject myPrefab;

    private void Start()
    {
        targetPosition = new Vector3(
            transform.position.x,
            transform.position.y + 3f,
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
            Instantiate(myPrefab, new Vector3(transform.position.x + 7, transform.position.y - 5, transform.position.z), Quaternion.identity);
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