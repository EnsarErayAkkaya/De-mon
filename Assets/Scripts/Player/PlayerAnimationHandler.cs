using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    public void CallPlayerAttackAnim()
    {
        animator.SetTrigger("attack");
    }
}
