using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCourse : MonoBehaviour
{
    [SerializeField] private RingBase[] rings;
    private float time = 0.0f;
    private int nextRing = 0;


    // Start is called before the first frame update
    void Start()
    {
        UpdateRings();
    }

    // Update is called once per frame
    void Update()
    {
        if (nextRing > 0)
        {
            time += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RingPassed(nextRing);
        }
    }

    internal void RingPassed(int ringNumber)
    {
        if (ringNumber == nextRing)
        {
            Debug.Log("ring " + ringNumber + " passed");
            if (ringNumber == 0)
            {

            }
            if (ringNumber < rings.Length - 1)
            {
                nextRing++;
            }
            else
            {
                nextRing = 0;
                print(time);
                time = 0.0f;
            }
            UpdateRings();
        }
    }

    private void UpdateRings()
    {
        foreach (RingBase ring in rings)
        {
            if (ring.ringID < nextRing)
            {
                ring.SetState(state.Passed);
            }else if (ring.ringID > nextRing)
            {
                ring.SetState(state.Normal);
            }
            else
            {
                ring.SetState(state.Next);
            }
        }
    }
}
