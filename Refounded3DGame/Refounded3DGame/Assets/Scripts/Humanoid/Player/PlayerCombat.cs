using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(PlayerFlags))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerCharacteristics))]
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerCombat : MonoBehaviour
    {
        private float _damage;
        private float _attackMovementSpeed = 0.7f;
        public float Damage => _damage;

        private PlayerFlags _playerFlags;
        private PlayerMovement _playerMovement;
        private PlayerCharacteristics _playerCharacteristics;
        private PlayerHealth _playerHealth;

        private AnimatorHandler _animatorHandler;
        private Animator _animator;

        private bool _isCombat = false;

        private void Start()
        {
            _playerHealth = GetComponent<PlayerHealth>();
            _playerFlags = GetComponent<PlayerFlags>();
            _animatorHandler = _playerFlags.animatorHandler;
            _animator = GetComponent<PlayerFlags>().animatorHandler.animator;
            _playerMovement = GetComponent<PlayerMovement>();
            _playerCharacteristics = GetComponent<PlayerCharacteristics>();
            _damage = _playerCharacteristics.Damage;
        }

        private void Update()
        {
            if (_playerHealth.IsDead) return;

            AttackCombo();
            _playerMovement.CombatMovement(_attackMovementSpeed);

            if (!_playerFlags.isGrounded || _playerFlags.isJumping) return;
            Block();

            if (_playerFlags.isInteracting) return;
            Attack();
            DrawSword();
        }

        private void DrawSword()
        {
            if (Input.GetButtonDown("DrawSword"))
            {
                _animatorHandler.PlayTargetAnimation("DrawSword", true, 0.1f);
            }
            var idle = _animator.GetFloat("Idle");
            if(idle == 1) _isCombat = true;
            if(idle == 0) _isCombat = false;

        }

        private void Attack()
        {
            if (Input.GetButtonDown("Attack") && !Input.GetButton("Roll") && !_playerFlags.isAttacking && _isCombat)
            {
                _animatorHandler.PlayTargetAnimation("Attack", true, 0.12f);
                _animator.SetBool("isAttacking", true);
            }
        }

        private void AttackCombo()
        {
            if (Input.GetButtonDown("Attack") && _isCombat)
            {
                if (_playerFlags.isAttacking && !Input.GetButton("Roll") && _playerFlags.isInteracting)
                {
                    _animator.SetBool("Attack", true);
                    _animator.SetBool("AttackCombo", true);
                }
            }
        }

        private void Block()
        {
            if(Input.GetButtonDown("Block") && !Input.GetButton("Roll") && !_playerMovement.IsRolling && _isCombat)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                _animator.SetBool("isBlocking", true);
                _animatorHandler.PlayTargetAnimation("Block", false, 0.12f);
            }
            if (Input.GetButtonUp("Block") && _isCombat || _playerMovement.IsRolling)
            {
                _animator.SetBool("isBlocking", false);

            }
        }
    }
}
