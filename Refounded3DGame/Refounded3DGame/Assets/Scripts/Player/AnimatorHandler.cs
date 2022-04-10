using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorHandler : MonoBehaviour
    {
        public Animator _animator;

        public bool _isInteracting;
        public bool _isJumping;
        public bool _isGrounded;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _isInteracting = _animator.GetBool("isInteracting");
            _isJumping = _animator.GetBool("isJumping");
            _isGrounded = _animator.GetBool("isGrounded");
        }

        public void UpdateAnimatorValues(Vector3 velocity)
        {
            Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
            float horizontal = horizontalVelocity.magnitude;
            float vertical = velocity.y;

            _animator.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
            _animator.SetFloat("Vertical", vertical, 0.05f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string name, bool isInteracting, float transitionTime)
        {
            _animator.SetBool("isInteracting", isInteracting);
            _animator.CrossFade(name, transitionTime);
        }
    }
}
