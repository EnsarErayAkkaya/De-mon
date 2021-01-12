using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Utility;

namespace Project.Player {
    public class PlayerMovement : MonoBehaviour
    {
        public float minSpeed;
        public float maxSpeed;
        float speed;
        public float acceleration;

        public float accelerateDirectionTreshold;

        public Camera cam;

        public Rigidbody2D rb;

        Vector2 movement;
        Vector2 mousePos;
        Vector2 lookDir;

        private void Update()
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");

            mousePos = FunctionLibrary.GetWorldPositionOnPlane(Input.mousePosition, 0);
            lookDir = mousePos - rb.position;

            //Get Normalized of Vectors for convinience
            Vector2 normalizedMovement = movement.normalized;
            Vector2 normalizedLookDir= lookDir.normalized;

            //If we are looking where we run then accelerate
            if (Mathf.Abs(normalizedMovement.y - normalizedLookDir.y) < accelerateDirectionTreshold 
                && Mathf.Abs(normalizedMovement.x - normalizedLookDir.x) < accelerateDirectionTreshold)
            {
                speed = Mathf.Lerp(speed, speed + acceleration, Time.deltaTime);
                speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
            }
            else
            {
                speed = Mathf.Lerp(speed, speed - acceleration, Time.deltaTime * 2 ); // slow down faster
                speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
            }
        }
        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);

            //look to mouse
            float angle = (Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg);
            rb.rotation = angle;
        }
    }
}
