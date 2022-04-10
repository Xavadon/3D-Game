using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private float _damage;
        public float Damage => _damage;


        private Rigidbody _rigidbody;
        private AnimatorHandler _animatorHandler;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        private void Update()
        {
            if (!_animatorHandler._isGrounded || _animatorHandler._isJumping || _animatorHandler._isInteracting)
                return;

            Attack();
            Block();
        }
        private void Attack()
        {
            if (Input.GetButtonDown("Attack"))
            {
                _rigidbody.velocity = Vector3.zero;
                _animatorHandler.PlayTargetAnimation("Attack", true, 0.1f);
                _animatorHandler._animator.SetFloat("Horizontal", 0);
                _animatorHandler._animator.SetFloat("Vertical", 0);
            }
        }

        private void Block()
        {
            //block
        }
    }
}
