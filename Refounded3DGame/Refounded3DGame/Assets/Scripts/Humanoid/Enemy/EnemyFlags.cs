using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class EnemyFlags : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        public AnimatorHandler animatorHandler;

        public bool isInteracting;
        public bool isJumping;
        public bool isGrounded;

        private void Update()
        {
            isInteracting = _animator.GetBool("isInteracting");
            isJumping = _animator.GetBool("isJumping");
            isGrounded = _animator.GetBool("isGrounded");
        }
    }
}
