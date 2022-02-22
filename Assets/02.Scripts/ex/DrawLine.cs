using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    Vector2 mousePosition;

    public Camera cam;
    public GameObject linePrefab;
    public GameObject currentLine;

    public LineRenderer lineRenderer;
    //public EdgeCollider2D edgeCollider;
    public List<Vector2> fingerPosition;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("GetMouseButtonDown");
            CreateLine();
        }

        if (Input.GetMouseButton(0))
        {
            //Debug.Log("GetMouseButton");
            Vector2 tempFingerPos = Input.mousePosition;
            Debug.Log(string.Format("tempFingerPos : {0}, fingerPosition[fingerPosition.Count - 1] : {1}", tempFingerPos, fingerPosition[fingerPosition.Count - 1]));
            if (Vector2.Distance(tempFingerPos, fingerPosition[fingerPosition.Count - 1]) > .1f)
            {
                UpdateLine(tempFingerPos);
            }
        }

        void CreateLine()
        {
            currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
            lineRenderer = currentLine.GetComponent<LineRenderer>();
            //edgeCollider = currentLine.GetComponent<EdgeCollider2D>();
            fingerPosition.Clear();
            fingerPosition.Add(Input.mousePosition);
            fingerPosition.Add(Input.mousePosition);
            lineRenderer.SetPosition(0, fingerPosition[0]);
            lineRenderer.SetPosition(1, fingerPosition[1]);
            //edgeCollider.points = fingerPosition.ToArray();
        }

        void UpdateLine(Vector2 newFingerPos)
        {
            fingerPosition.Add(newFingerPos);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
        }
    }
}
