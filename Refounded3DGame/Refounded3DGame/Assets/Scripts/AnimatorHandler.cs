using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorHandler : MonoBehaviour
    {
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void UpdateAnimatorValues(Vector3 velocity)
        {
            Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
            float horizontal = horizontalVelocity.magnitude;
            float vertical = velocity.y;

            _animator.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
            _animator.SetFloat("Vertical", vertical, 0.05f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string name)
        {
            _animator.CrossFade(name, 0.1f);
        }
    }
}
