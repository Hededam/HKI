using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform targetToFollow;
    public Vector3 followOffset;
    
    void Start()
    {
        followOffset = new Vector3 (0, 0, 0);
    }

    
    void Update()
    {
        transform.position = targetToFollow.position += followOffset;
    }
}
