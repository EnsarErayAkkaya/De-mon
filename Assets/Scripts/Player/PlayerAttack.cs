﻿using Project.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private Transform attackPoint;
        [SerializeField] private PlayerAnimationHandler playerAnimationHandler;
        [SerializeField] private float pushForce;
        [SerializeField] private float attackRange;
        [SerializeField] private LayerMask enemyLayers;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Attack
                Attack();
            }
        }
        private void Attack()
        {
            // start attack animation
            playerAnimationHandler.CallPlayerAttackAnim();

            //detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyBase>().PushEnemy(pushForce);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}