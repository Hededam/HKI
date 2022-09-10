using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_testScript : MonoBehaviour
{
    public CapsuleCollider playerCollider;
    public float moveSpeed = 5f;

    private GameObject enemy;
    private Enemy_testScript enemyScript;


    // Start is called before the first frame update
    void Start()
    {
        //ændre størelse på collider 
        playerCollider = GetComponent<CapsuleCollider>();
        playerCollider.height = 1f;
        playerCollider.center = new Vector3(0f, 0.5f, 0f);


        enemy = GameObject.Find("Enemy_cupe");
        enemyScript = enemy.GetComponent<Enemy_testScript>();
    }

    // Update is called once per frame
    void Update()
    {

        // Move the player
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        transform.Translate(movement * Time.deltaTime * moveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            enemyScript.enemyHelth--;
        }
    }
}
 