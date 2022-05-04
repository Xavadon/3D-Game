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
        public float Damage => _damage;


        private PlayerFlags _playerFlags;
        private Rigidbody _rigidbody;

        private AnimatorHandler _animatorHandler;

        private void Start()
        {
            _playerFlags = GetComponent<PlayerFlags>();
            _animatorHandler = _playerFlags.animatorHandler;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!_playerFlags.isGrounded || _playerFlags.isJumping || _playerFlags.isInteracting)
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
                _animatorHandler.animator.SetFloat("Horizontal", 0);
                _animatorHandler.animator.SetFloat("Vertical", 0);
            }
        }

        private void Block()
        {
            //block
        }
    }
}
