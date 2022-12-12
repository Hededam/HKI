using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using RingCourse;

/* TODO:
 * fix undo removing multiple references in manager.rings when multiple are added.
 * 
 * 
 */

[CustomEditor(typeof(RingCourseManager))]
//[EditorTool("Ring Course Tool", typeof(RingCourseManager))]
class RingCourseEditor : Editor//, IDrawSelectedHandles
{
    private RingCourseManager manager;
    [SerializeField]
    public GameObject ringPrefab;

    void OnEnable()
    {
    }

    public void OnSceneGUI()
    {
        manager = (RingCourseManager)target;
        //foreach (CourseRing ring in manager.rings)
        //{
        //    Handles.TransformHandle(ring.transform.position, ring.transform.rotation, ring.transform.localScale);
        //}

        Handles.color = Color.cyan;
        for (int i = 0; i <= manager.rings.Count; i++)
        {
            Vector3 pos1 = i > 0 ? RingPosition(i - 1) : RingPosition(i) + (RingPosition(i) - RingPosition(i + 1)).normalized * 5f;
            Vector3 pos2 = i < manager.rings.Count ? RingPosition(i) : RingPosition(i - 1) + (RingPosition(i - 1) - RingPosition(i - 2)).normalized * 5f;

            Vector3 buttonPos = (pos2 - pos1) * 0.5f + pos1;
            float buttonSize = HandleUtility.GetHandleSize(buttonPos) * 0.2f;
            if (i > 0 && i < manager.rings.Count)
            {
                Handles.DrawLine(pos1, pos2, 3.0f);
            }
            if (Handles.Button(buttonPos, SceneView.currentDrawingSceneView.rotation, buttonSize, buttonSize + 0.5f, Handles.DotHandleCap))
            {
                Debug.Log($"Clicked between ring {i - 1} and {i}");
                AddRing(i);
            }
        }
    }

    public void AddRing(int index)
    {
        Vector3 pos = Vector3.zero;
        if (index == 0)
        {
            Debug.Log("start");
            pos = RingPosition(index) + (RingPosition(index) - RingPosition(index + 1)).normalized * 5f;
        }
        else if (index == manager.rings.Count)
        {
            Debug.Log("end");
            pos = RingPosition(index - 1) + (RingPosition(index - 2) - RingPosition(index - 1)).normalized * 5f;
        }
        else
        {
            Debug.Log("in between");
            pos = (RingPosition(index) - RingPosition(index - 1)) * 0.5f;
        }
        CourseRing newRing = Instantiate(ringPrefab, pos, Quaternion.LookRotation(pos), manager.transform).GetComponent<CourseRing>();
        newRing.ringID = index;
        newRing.transform.SetSiblingIndex(index);
        Undo.RegisterCreatedObjectUndo(newRing.gameObject, "Add Ring");
        Undo.RecordObject(manager, "Add Ring");
        manager.rings.Insert(index, newRing);
        for (int i = index + 1; i < manager.rings.Count; i++)
        {
            Undo.RecordObject(manager.rings[i], "Add Ring");
            manager.rings[i].ringID = i;
        }
    }

    public void RemoveRing(int index)
    {

    }

    private Vector3 RingPosition(int i)
    {
        return manager.rings[i].transform.position;
    }
}