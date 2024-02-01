using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using RingCourse;

/* TODO:
 * Implement RemoveRing.
 * easier access to ring transforms.
 * 
 * Optimization?
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
        Event e = Event.current;
        manager = (RingCourseManager)target;
        //foreach (CourseRing ring in manager.rings)
        //{
        //    Handles.TransformHandle(ring.transform.position, ring.transform.rotation, ring.transform.localScale);
        //}

        Handles.color = Color.cyan;
        for (int i = 0; i <= manager.rings.Count; i++)
        {
            Vector3 pos1 = i > 0 ? RingPosition(i - 1) : RingPosition(i) + (RingPosition(i) - RingPosition(i + 1)).normalized * 6f;
            Vector3 pos2 = i < manager.rings.Count ? RingPosition(i) : RingPosition(i - 1) + (RingPosition(i - 1) - RingPosition(i - 2)).normalized * 6f;

            if (i > 0 && i < manager.rings.Count)
            {
                Handles.DrawLine(pos1, pos2, 3.0f);
            }
            if (!e.shift)
            {
                Vector3 buttonPos = (pos2 - pos1) * 0.5f + pos1;
                float buttonSize = HandleUtility.GetHandleSize(buttonPos) * 0.2f;
                if (Handles.Button(buttonPos, SceneView.currentDrawingSceneView.rotation, buttonSize, buttonSize + 0.5f, Handles.DotHandleCap))
                {
                    AddRing(i);
                }
            }
            else if (i < manager.rings.Count)
            {
                Vector3 buttonPos = pos2;
                float buttonSize = HandleUtility.GetHandleSize(buttonPos) * 0.2f;

                Handles.color = Color.red;
                if (Handles.Button(buttonPos, SceneView.currentDrawingSceneView.rotation, buttonSize, buttonSize + 0.5f, Handles.DotHandleCap))
                {
                    RemoveRing(i);
                }
                Handles.color = Color.cyan;
            }
        }
    }

    public void AddRing(int index)
    {
        Vector3 pos = Vector3.zero;
        if (index == 0)
        {
            pos = RingPosition(index) + (RingPosition(index) - RingPosition(index + 1)).normalized * 3f;
        }
        else if (index == manager.rings.Count)
        {
            pos = RingPosition(index - 1) + (RingPosition(index - 1) - RingPosition(index - 2)).normalized * 3f;
        }
        else
        {
            pos = RingPosition(index - 1) + (RingPosition(index) - RingPosition(index - 1)) * 0.5f;
        }
        CourseRing newRing = Instantiate(ringPrefab, pos, Quaternion.LookRotation(pos), manager.transform).GetComponent<CourseRing>();
        newRing.transform.SetSiblingIndex(index);
        Undo.RegisterCreatedObjectUndo(newRing.gameObject, "Add Ring");
        Undo.RegisterCompleteObjectUndo(manager, "Add Ring");
        manager.rings.Insert(index, newRing);
    }

    public void RemoveRing(int index)
    {
        //Undo.RegisterCompleteObjectUndo(manager, "Remove Ring");
        //Undo.DestroyObjectImmediate();
        //manager.rings.RemoveAt(index);
    }

    private Vector3 RingPosition(int i)
    {
        return manager.rings[i].transform.position;
    }
}