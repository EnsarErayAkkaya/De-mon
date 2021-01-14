using Project.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Player
{
    public class GrabberLine : MonoBehaviour
    {
        [Header("Referances")]
        [SerializeField] private LineRenderer line;
        [SerializeField] private Transform player;

        [Header("Data")]
        [SerializeField] private int pointCount;
        [SerializeField] private float maxDistanceBetweenPoints;
        [SerializeField] private Vector2 boxCastSize;
        [SerializeField] private float lineWidth;
        [SerializeField] private LayerMask obstacle;

        [Tooltip("How many time calculations repeat in one update")]
        [SerializeField] private int calculationCount;

        public List<LinePoint> points;
        private Vector2 mousePos;

        private void Start()
        {
            line = GetComponentInChildren<LineRenderer>();
            points = new List<LinePoint>();

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
            for (int j = 0; j < calculationCount; j++)
            {
                for (int i = 0; i < pointCount; i++)
                {
                    LinePoint currentPoint = points[i];

                    if (i == 0) // first point will follow player
                    {
                        currentPoint.posNow = player.position;
                    }
                    else if (i == pointCount - 1) // last point
                    {
                        LinePoint previousPoint = points[i - 1];

                        // check if distance between previousPoint and current point bigger then max dist.
                        if ((previousPoint.posNow - currentPoint.posNow).magnitude > maxDistanceBetweenPoints)
                        {
                            // if it is go to previousPoint
                            currentPoint = MovePointToPos(currentPoint, previousPoint.posNow);
                        }
                        else if ((previousPoint.posNow - currentPoint.posNow).magnitude < maxDistanceBetweenPoints)
                        {
                            // else go to mousePos
                            currentPoint = MovePointToPos(currentPoint, mousePos);
                        }
                    }
                    else // point in between
                    {
                        LinePoint previousPoint = points[i - 1];
                        LinePoint nextPoint = points[i + 1];

                        // check if distance between previousPoint and current point bigger then max dist.
                        if ((previousPoint.posNow - currentPoint.posNow).magnitude >= maxDistanceBetweenPoints)
                        {
                            // if it is bigger then max dist go to previous point
                            currentPoint = MovePointToPos(currentPoint, previousPoint.posNow);
                        }
                        else if ((previousPoint.posNow - currentPoint.posNow).magnitude < maxDistanceBetweenPoints)
                        {
                            // else go to next point
                            currentPoint = MovePointToPos(currentPoint, nextPoint.posNow);
                        }

                    } // else point in between
                    points[i] = currentPoint;

                } // for each point
            } // for calculation count
        }
        private void DrawRope()
        {
            // set line width
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;

            // convert positions to array
            Vector3[] Positions = new Vector3[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                Positions[i] = points[i].posNow;
            }

            // set line positions
            line.positionCount = Positions.Length;
            line.SetPositions(Positions);
        }

        /// <summary>
        /// Checks all points and returns closest point index to collisionPoint
        /// </summary>
        /// <param name="collisionPoint"></param>
        /// <returns>closest points index</returns>
        private LinePoint MovePointToPos(LinePoint currentPoint, Vector2 targetPos)
        {
            // get vector between positions
            var difference = targetPos - currentPoint.posNow;
            // normalize to only get a direction with magnitude = 1
            var direction = difference.normalized;
            // here you "clamp" use the smaller of either
            // the max radius or the magnitude of the difference vector
            var distance = Mathf.Min(maxDistanceBetweenPoints, difference.magnitude);

            RaycastHit2D hitResult;
            hitResult = Physics2D.BoxCast(currentPoint.posNow, boxCastSize, 0f, direction, distance, obstacle);

            if (hitResult.collider != null)
            {
                float xStart = direction.x, yStart = direction.y;
                direction += hitResult.normal;
                if (Math.Sign(direction.x) != Math.Sign(xStart))
                    direction.x = 0;
                if (Math.Sign(direction.y) != Math.Sign(yStart))
                    direction.y = 0;
            }

            currentPoint.posNow += direction * distance;
            return currentPoint;
        }
    }
    [Serializable]
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
}