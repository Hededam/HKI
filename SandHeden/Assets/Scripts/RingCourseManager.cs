using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RingCourse
{
    public class RingCourseManager : MonoBehaviour
    {
        //[HideInInspector]
        public List<CourseRing> rings;

        [HideInInspector]
        public bool courseStarted = false;
        [HideInInspector]
        public float time = 0.0f;
        private int nextRing = 0;
        // Start is called before the first frame update
        void Start()
        {
            foreach(CourseRing ring in GetComponentsInChildren<CourseRing>())
            {
                rings.Add(ring);
            }
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
                nextRing = (nextRing + 1) % rings.Count;
                if (ringNumber == 0)
                {
                    courseStarted = true;
                    StartCoroutine(Timer());
                }
                if (ringNumber >= rings.Count - 1)
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

        private void UpdateRing(CourseRing ring)
        {
            int ringIndex = ring.transform.GetSiblingIndex();
            if (ringIndex < nextRing)
            {
                ring.SetState(State.Passed);
            }
            else if (ringIndex > nextRing)
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
            foreach (CourseRing ring in rings)
            {
                UpdateRing(ring);
            }
        }
    }
}
