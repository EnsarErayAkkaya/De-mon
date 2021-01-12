using Project.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberLine : MonoBehaviour
{
    [Header("Referances")]
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform player;
    [Header("Data")]
    [SerializeField] private List<LinePoint> points = new List<LinePoint>();
    [SerializeField] private int pointCount;
    [SerializeField] private float maxDistanceBetweenPoints;
    [SerializeField] private float lineWidth;

    private Vector2 mousePos;
    private void Start()
    {
        line = GetComponentInChildren<LineRenderer>();
        //collider = GetComponent<Collider>();

        line.positionCount = pointCount;
        for (int i = 0; i < pointCount; i++)
        {
            this.points.Add(new LinePoint(player.position));
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
            this.DrawRope();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
            FollowMouse();
    }

    private void FollowMouse()
    {
        mousePos = FunctionLibrary.GetWorldPositionOnPlane(Input.mousePosition, 0);
        for (int i = 0; i < length; i++)
        {

        }
        for (int i = 0; i < pointCount; i++)
        {
            LinePoint currentPoint = points[i];

            if(i == 0) // first point will follow player
            {
                currentPoint.posOld = currentPoint.posNow;
                currentPoint.posNow = player.position;
            }
            else if (i == pointCount - 1) // last point
            {
                LinePoint previousPoint = points[i - 1];
                // check if distance between previousPoint and current point bigger then max dist.
                if ((previousPoint.posNow - currentPoint.posNow).magnitude >= maxDistanceBetweenPoints)
                {
                    // if it is bigger then max dist go to previous point
                    currentPoint.posOld = currentPoint.posNow;
                    // get vector between positions
                    var difference = previousPoint.posNow - currentPoint.posNow;
                    // normalize to only get a direction with magnitude = 1
                    var direction = difference.normalized;
                    // here you "clamp" use the smaller of either
                    // the max radius or the magnitude of the difference vector
                    var distance = Mathf.Min(maxDistanceBetweenPoints, difference.magnitude);
                    // and finally apply the end position
                    currentPoint.posNow += direction * distance;
                }
                else if ((previousPoint.posNow - currentPoint.posNow).magnitude < maxDistanceBetweenPoints)
                {
                    // else go to mousePos

                    currentPoint.posOld = currentPoint.posNow;
                    // get vector between positions
                    var difference = mousePos - currentPoint.posNow;
                    // normalize to only get a direction with magnitude = 1
                    var direction = difference.normalized;
                    // here you "clamp" use the smaller of either
                    // the max radius or the magnitude of the difference vector
                    var distance = Mathf.Min(maxDistanceBetweenPoints, difference.magnitude);

                    Debug.Log("mouse direction * distance:" + distance * direction);
                    // and finally apply the end position
                    currentPoint.posNow += direction * distance;
                    Debug.Log("currentPoint.posNow:" + currentPoint.posNow);
                }

                points[i - 1] = previousPoint;

            }
            else // point in between
            {
                LinePoint previousPoint = points[i - 1];
                LinePoint nextPoint = points[i + 1];
                // check if distance between previousPoint and current point bigger then max dist.
                if ((previousPoint.posNow - currentPoint.posNow).magnitude >= maxDistanceBetweenPoints)
                {
                    // if it is bigger then max dist go to previous point
                    currentPoint.posOld = currentPoint.posNow;
                    // get vector between positions
                    var difference = previousPoint.posNow - currentPoint.posNow;
                    // normalize to only get a direction with magnitude = 1
                    var direction = difference.normalized;
                    // here you "clamp" use the smaller of either
                    // the max radius or the magnitude of the difference vector
                    var distance = Mathf.Min(maxDistanceBetweenPoints, difference.magnitude);
                    // and finally apply the end position
                    currentPoint.posNow += direction * distance;
                }
                else if((previousPoint.posNow - currentPoint.posNow).magnitude < maxDistanceBetweenPoints)
                {
                    // else go to next point

                    currentPoint.posOld = currentPoint.posNow;
                    // get vector between positions
                    var difference = nextPoint.posNow - currentPoint.posNow;
                    // normalize to only get a direction with magnitude = 1
                    var direction = difference.normalized;
                    // here you "clamp" use the smaller of either
                    // the max radius or the magnitude of the difference vector
                    var distance = Mathf.Min(maxDistanceBetweenPoints, difference.magnitude);
                    // and finally apply the end position
                    currentPoint.posNow += direction * distance;
                }

                points[i - 1] = previousPoint;
                points[i + 1] = nextPoint;

            } // else

            points[i] = currentPoint;

        } // for
    }
    private void DrawRope()
    {
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        Vector3[] Positions = new Vector3[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            Positions[i] = points[i].posNow;
        }

        line.positionCount = Positions.Length;
        line.SetPositions(Positions);
    }
}
public struct LinePoint
{
    public Vector2 posOld;
    public Vector2 posNow;
    public LinePoint(Vector2 pos)
    {
        this.posOld = pos;
        this.posNow = pos;
    }
}