using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Player;

namespace Project.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;
        protected PlayerMaster player;
        [SerializeField] protected float detectionRange;
        private Vector3 pushStartPoint;
        [SerializeField] private float pushDistance;
        private bool isPushed;

        public LayerMask obstacle;

        private void Start()
        {
            player = FindObjectOfType<PlayerMaster>();
        }
        private void Update()
        {
            if(isPushed)
            {
                if(Vector3.Distance(pushStartPoint,transform.position) >= pushDistance)
                {
                    StopPush();
                }
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(1<<collision.gameObject.layer == (obstacle.value & 1<< collision.gameObject.layer) && isPushed)
            {
                Die();
            }
        }
        protected bool IsPlayerInRange()
        {
            return Vector3.Distance(player.transform.position, transform.position) < detectionRange;
        }
        public void PushEnemy(float pushForce)
        {
            pushStartPoint = transform.position;

            Vector3 direction = -(player.transform.position - transform.position);
            rb.velocity = direction * pushForce;

            isPushed = true;
        }
        private void StopPush()
        {
            isPushed = false;
            rb.velocity = Vector2.zero;
        }
        private void Die()
        {
            StopPush();
            Debug.Log(gameObject.name + " is dead");
            Destroy(gameObject);
        }
    }
}
