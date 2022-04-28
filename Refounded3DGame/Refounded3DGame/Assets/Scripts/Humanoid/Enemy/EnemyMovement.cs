using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class EnemyMovement : MonoBehaviour
    {
        private Transform _target;

        [Header("Movement")]
        [SerializeField] private float _moveSpeed;
        private Vector3 _moveDirection;

        [Header("Distances")]
        [SerializeField] private float _distanceToChase;
        public float distanceToStop;

        [Header("Rotation")]
        [SerializeField] private float _rotationTime;
        private float _rotationVelocity;

        private Quaternion _rotationAngle;

        private AnimatorHandler _animatorHandler;
        private Rigidbody _rigidbody;
        private EnemyHealth _enemyHealth;

        private void Start()
        {
            _animatorHandler = GetComponentInChildren<AnimatorHandler>();
            _rigidbody = GetComponent<Rigidbody>();
            _enemyHealth = GetComponent<EnemyHealth>();
            _target = Player.singleton.transform;
        }

        private void Update()
        {
            if (_enemyHealth.IsDead)
                return;

            Move();
        }

        private void Move()
        {
            Vector3 direction = _target.transform.position - transform.position;
            direction.Normalize();

            if (Vector3.Distance(_target.position, transform.position) < _distanceToChase)
            {
                ApplyRotation(direction);
                if (Vector3.Distance(_target.position, transform.position) < distanceToStop)
                    _rigidbody.velocity = Vector3.zero;
                else
                {
                    _moveDirection = _rotationAngle * Vector3.forward * _moveSpeed;
                    _rigidbody.velocity = _moveDirection;
                }
            }

            _animatorHandler.UpdateAnimatorValues(_rigidbody.velocity);
        }
        private void ApplyRotation(Vector3 direction)
        {
            if (direction.magnitude > 0.1f)
            {
                float _targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _rotationVelocity, _rotationTime);
                _rotationAngle = Quaternion.Euler(0, rotationAngle, 0);
                transform.rotation = _rotationAngle;
            }
        }

    }
}
