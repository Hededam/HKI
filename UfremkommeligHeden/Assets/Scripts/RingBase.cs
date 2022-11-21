using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RingCourse
{
    public enum State
    {
        Normal,
        Next,
        Passed
    }

    public class RingBase : MonoBehaviour
    {
        private RingCourseManager course;
        private State currentState = State.Normal;
        private MeshRenderer meshRenderer;
        [SerializeField] private Material stateMatNormal;
        [SerializeField] private Material stateMatNext;
        [SerializeField] private Material stateMatPassed;

        public int ringID;

        // Start is called before the first frame update
        void Start()
        {
            course = transform.parent.GetComponent<RingCourseManager>();
            meshRenderer = GetComponent<MeshRenderer>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                course.RingPassed(ringID);
            }
        }

        public void SetState(State newState)
        {
            if (newState == currentState) return;
            currentState = newState;
            switch (newState)
            {
                case State.Normal:
                    meshRenderer.material = stateMatNormal;
                    break;
                case State.Next:
                    meshRenderer.material = stateMatNext;
                    break;
                case State.Passed:
                    meshRenderer.material = stateMatPassed;
                    break;
                default:
                    Debug.LogError("Invalid State");
                    break;
            }
        }
    }
}