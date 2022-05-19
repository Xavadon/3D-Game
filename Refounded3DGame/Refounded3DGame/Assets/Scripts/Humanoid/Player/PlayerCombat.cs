using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(PlayerFlags))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _attackMovementSpeed;
        public float Damage => _damage;


        private PlayerFlags _playerFlags;
        private PlayerMovement _playerMovement;
        private Rigidbody _rigidbody;

        private AnimatorHandler _animatorHandler;
        private Animator _animator;

        private void Start()
        {
            _playerFlags = GetComponent<PlayerFlags>();
            _animatorHandler = _playerFlags.animatorHandler;
            _animator = GetComponent<PlayerFlags>().animatorHandler.animator;
            _rigidbody = GetComponent<Rigidbody>();
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            AttackCombo();
            _playerMovement.CombatMovement(_attackMovementSpeed);

            if (!_playerFlags.isGrounded || _playerFlags.isJumping || _playerFlags.isInteracting)
                return;

            Attack();
            Block();
        }
        private void Attack()
        {
            if (Input.GetButtonDown("Attack") && !Input.GetButton("Roll") && !_playerFlags.isAttacking)
            {
                _animatorHandler.PlayTargetAnimation("Attack", true, 0.12f);
                _animator.SetBool("isAttacking", true);
            }
        }

        private void AttackCombo()
        {
            if (Input.GetButtonDown("Attack"))
            {
                if (_playerFlags.isAttacking && _playerFlags.isInteracting)
                {
                    _animator.SetBool("Attack", true);
                    _animator.SetBool("AttackCombo", true);
                }
            }
        }

        private void Block()
        {
            //block
        }
    }
}
