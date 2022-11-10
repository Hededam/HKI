using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum state{
    Normal,
    Next,
    Passed
}

public class RingBase : MonoBehaviour
{
    private RingCourse course;
    private state currentState = state.Normal;
    private MeshRenderer meshRenderer;
    [SerializeField] private Material stateMatNormal;
    [SerializeField] private Material stateMatNext;
    [SerializeField] private Material stateMatPassed;

    public int ringID;

    // Start is called before the first frame update
    void Start()
    {
        course = transform.parent.GetComponent<RingCourse>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            course.RingPassed(ringID);
        }
    }

    public void SetState(state newState)
    {
        if (newState == currentState) return;
        currentState = newState;
        switch (newState)
        {
            case state.Normal:
                meshRenderer.material = stateMatNormal;
                break;
            case state.Next:
                meshRenderer.material = stateMatNext;
                break;
            case state.Passed:
                meshRenderer.material = stateMatPassed;
                break;
            default:
                print("why?");
                break;
        }
    }
}
