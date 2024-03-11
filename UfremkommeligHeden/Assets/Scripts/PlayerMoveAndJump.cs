using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMoveAndJump : MonoBehaviour
{
    
    new Vector3 _originalPosition;
    public Rigidbody rb;
    public float jumpAmount = 5f;

    public float vSpeed = 10.0f;
    public float hSpeed = 10.0f;

    private void Start()
    {
        _originalPosition = new Vector3();
         _originalPosition = transform.forward;
    }

    void Update()
    {
        float vTranslation = Input.GetAxis("Vertical") * vSpeed;
        float hTranslation = Input.GetAxis("Horizontal") * hSpeed;

        vTranslation *= Time.deltaTime;
        hTranslation *= Time.deltaTime;

        transform.Translate(0, 0, vTranslation);

        transform.Translate(hTranslation, 0, 0);

        bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, 1.2f);
        }


        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
            transform.forward = _originalPosition;
        }

        if (transform.position.y <= -10)
        {
            SceneManager.LoadScene("Jumping-scene");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Kill"))
        {
            SceneManager.LoadScene("Jumping-scene");
        }
    }
}