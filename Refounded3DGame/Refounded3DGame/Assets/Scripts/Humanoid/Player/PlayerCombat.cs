using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(PlayerFlags))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _attackMovementSpeed;
        public float Damage => _damage;


        private PlayerFlags _playerFlags;
        private Rigidbody _rigidbody;

        private AnimatorHandler _animatorHandler;
        private Animator _animator;

        private void Start()
        {
            _playerFlags = GetComponent<PlayerFlags>();
            _animatorHandler = _playerFlags.animatorHandler;
            _animator = _animatorHandler.animator;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            AttackCombo();
            
            if (!_playerFlags.isGrounded || _playerFlags.isJumping || _playerFlags.isInteracting)
                return;

            Attack();
            Block();
        }
        private void Attack()
        {
            if (Input.GetButtonDown("Attack") && !Input.GetButton("Roll"))
            {
                _rigidbody.velocity = Vector3.zero;
                _animatorHandler.PlayTargetAnimation("Attack", true, 0.12f);

                SetAnimatorValues();
            }
        }

        private void AttackCombo()
        {
            if (Input.GetButtonDown("Attack"))
            {
                if (_playerFlags.isAttacking && _playerFlags.isInteracting)
                {
                    _animator.SetBool("Attack", true);
                    _rigidbody.velocity = transform.forward * _attackMovementSpeed;
                }
            }
        }

        private void SetAnimatorValues()
        {
            _animator.SetBool("isAttacking", true);
            _animator.SetFloat("Horizontal", 0);
            _animator.SetFloat("Vertical", 0);
        }

        private void Block()
        {
            //block
        }
    }
}
