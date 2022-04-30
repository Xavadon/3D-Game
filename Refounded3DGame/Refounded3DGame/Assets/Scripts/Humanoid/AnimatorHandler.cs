using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorHandler : MonoBehaviour
    {
        [HideInInspector] public Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void UpdateAnimatorValues(Vector3 velocity)
        {
            Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
            float horizontal = horizontalVelocity.magnitude;
            float vertical = velocity.y;

            animator.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
            animator.SetFloat("Vertical", vertical, 0.05f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string name, bool isInteracting, float transitionTime)
        {
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(name, transitionTime);
        }
    }
}
