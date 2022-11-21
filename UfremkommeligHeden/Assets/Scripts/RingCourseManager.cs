using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RingCourse
{
    public class RingCourseManager : MonoBehaviour
    {
        public RingBase[] rings;
        private float time = 0.0f;
        private int nextRing = 0;
        public bool courseStarted = false;

        // Start is called before the first frame update
        void Start()
        {
            UpdateRings();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RingPassed(nextRing);
            }
        }

        private IEnumerator Timer()
        {
            while (courseStarted)
            {
                time += Time.deltaTime;
                yield return null;
            }
            time = 0.0f;
        }

        internal void RingPassed(int ringNumber)
        {
            if (ringNumber == nextRing)
            {
                int prevRing = nextRing;
                nextRing = (nextRing + 1) % rings.Length;
                if (ringNumber == 0)
                {
                    courseStarted = true;
                    StartCoroutine(Timer());
                }
                if (ringNumber >= rings.Length - 1)
                {
                    print(time);
                    courseStarted = false;
                    UpdateRings();
                }
                else
                {
                    UpdateRing(rings[prevRing]);
                    UpdateRing(rings[nextRing]);
                }
            }
        }

        private void UpdateRing(RingBase ring)
        {
            if (ring.ringID < nextRing)
            {
                ring.SetState(State.Passed);
            }
            else if (ring.ringID > nextRing)
            {
                ring.SetState(State.Normal);
            }
            else
            {
                ring.SetState(State.Next);
            }
        }

        private void UpdateRings()
        {
            foreach (RingBase ring in rings)
            {
                UpdateRing(ring);
            }
        }
    }
}
