using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class PlayerFlags : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        public AnimatorHandler animatorHandler;

        public bool isInteracting;
        public bool isJumping;
        public bool isGrounded;
        public bool isAttacking;
        public bool isBlocking;
        public bool canTakeDamage = true;

        private void Update()
        {
            isInteracting = _animator.GetBool("isInteracting");
            isJumping = _animator.GetBool("isJumping");
            isGrounded = _animator.GetBool("isGrounded");
            isAttacking = _animator.GetBool("isAttacking");
            isBlocking = _animator.GetBool("isBlocking");
        }
    }
}
