using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using RingCourse;

[CustomEditor(typeof(RingCourseManager))]
[EditorTool("Ring Course Tool", typeof(RingCourseManager))]
class RingCourseEditor : EditorTool, IDrawSelectedHandles
{
    private RingCourseManager manager;

    void OnEnable()
    {
        var manager = target as RingCourseManager;

        Debug.Log(manager.rings);
    }

    public override void OnToolGUI(EditorWindow window)
    {

    }

    public void OnDrawHandles()
    {
        //for (int i = 1; i < manager.rings.Length; i++)
        //{
        //    Handles.DrawLine(manager.rings[i - 1].transform.position, manager.rings[i].transform.position, 3);
        //}
    }
}