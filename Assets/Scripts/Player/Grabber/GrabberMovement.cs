using Project.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Player
{
    public class GrabberMovement : MonoBehaviour
    {
        Rigidbody2D rb;
        [SerializeField] private float grabberSpeed;
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Vector2 mousePos = FunctionLibrary.GetWorldPositionOnPlane(Input.mousePosition, 0);
            
            if (((Vector2)rb.position - mousePos).magnitude < 0.1f) return;

            Vector2 dir = (mousePos - (Vector2)transform.position).normalized;
            rb.MovePosition(rb.position + dir * grabberSpeed * Time.deltaTime);
        }
    }
}