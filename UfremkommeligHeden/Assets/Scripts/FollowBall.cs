using UnityEngine;

public class FollowBall : MonoBehaviour
{
    public Transform ball; // Assign this in the Inspector
    public Vector3 offset; // Offset if needed

    void LateUpdate()
    {
        transform.position = ball.position + offset;
    }
}
