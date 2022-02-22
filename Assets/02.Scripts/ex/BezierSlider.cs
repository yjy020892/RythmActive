using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BezierSlider : MonoBehaviour
{
    public LineRenderer lr;

    public GameObject hitSlider;
    public GameObject obj;

    public Vector2 P1, P2, P3, P4;

    public int vertexCount = 12;

    [Range(0, 1)]
    public float value;

    private void Update()
    {
        obj.transform.position = CreateBezier(P1, P2, P3, P4, value);

        var pointList = new List<Vector3>();
        for(float i = 0; i <= 1; i += 1.0f / vertexCount)
        {
            //Vector2 before = Vector2.Lerp(P1, P2, i);

            //Vector2 after = Vector2.Lerp(P2, P3, i);

            //Vector2 bezierPoint = Vector2.Lerp(before, after, i);

            pointList.Add(CreateBezier(P1, P2, P3, P4, i));
        }

        lr.positionCount = pointList.Count;
        lr.SetPositions(pointList.ToArray());
    }

    public Vector2 CreateBezier(Vector2 P1, Vector2 P2, Vector2 P3, Vector2 P4, float value)
    {
        Vector2 A = Vector2.Lerp(P1, P2, value);
        Vector2 B = Vector2.Lerp(P2, P3, value);
        Vector2 C = Vector2.Lerp(P3, P4, value);

        Vector2 D = Vector2.Lerp(A, B, value);
        Vector2 E = Vector2.Lerp(B, C, value);

        Vector2 F = Vector2.Lerp(D, E, value);

        return F;
    }
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(BezierSlider))]
public class BezierEditor : Editor
{
    private void OnSceneGUI()
    {
        BezierSlider bezierSlider = (BezierSlider)target;

        bezierSlider.P1 = Handles.PositionHandle(bezierSlider.P1, Quaternion.identity);
        bezierSlider.P2 = Handles.PositionHandle(bezierSlider.P2, Quaternion.identity);
        bezierSlider.P3 = Handles.PositionHandle(bezierSlider.P3, Quaternion.identity);
        bezierSlider.P4 = Handles.PositionHandle(bezierSlider.P4, Quaternion.identity);

        Handles.DrawLine(bezierSlider.P1, bezierSlider.P2);
        Handles.DrawLine(bezierSlider.P3, bezierSlider.P4);

        int count = 30;
        for(float i = 0; i < count; i++)
        {
            float value_Before = i / count;
            Vector2 before = bezierSlider.CreateBezier(bezierSlider.P1, bezierSlider.P2, bezierSlider.P3, bezierSlider.P4, value_Before);

            float value_After = (i + 1) / count;
            Vector2 after = bezierSlider.CreateBezier(bezierSlider.P1, bezierSlider.P2, bezierSlider.P3, bezierSlider.P4, value_After);

            Handles.DrawLine(before, after);
        }
    }
}
#endif
