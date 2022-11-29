using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using RingCourse;

//[CustomEditor(typeof(RingCourseManager))]
[EditorTool("Ring Course Tool", typeof(RingCourseManager))]
class RingCourseEditor : EditorTool, IDrawSelectedHandles
{
    private RingCourseManager manager;

    void OnEnable()
    {
        manager = target as RingCourseManager;
    }

    public void OnSceneGUI()
    {
        //foreach (RingBase ring in manager.rings)
        //{
        //    Handles.TransformHandle(ring.transform.position, ring.transform.rotation, ring.transform.localScale);
        //}
    }

    public void OnDrawHandles()
    {
        Handles.color = Color.cyan;
        for (int i = 1; i < manager.rings.Length; i++)
        {
            Vector3 pos1 = manager.rings[i - 1].transform.position;
            Vector3 pos2 = manager.rings[i].transform.position;
            Vector3 buttonPos = (pos2 - pos1) * 0.5f + pos1;
            float buttonSize = HandleUtility.GetHandleSize(buttonPos) * 0.2f;

            Handles.DrawLine(pos1, pos2, 3.0f);
            if(Handles.Button(buttonPos, SceneView.currentDrawingSceneView.rotation, buttonSize, buttonSize + 0.5f, Handles.RectangleHandleCap))
            {
                Debug.Log("test");
            }
        }
    }
}