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

        [Header("Rotation")]
        [SerializeField] private float _rotationTime;
        private float _rotationVelocity;
        private Quaternion _rotationAngle;

        private AnimatorHandler _animatorHandler;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _animatorHandler = GetComponentInChildren<AnimatorHandler>();
            _rigidbody = GetComponent<Rigidbody>();
            _target = PlayerStats._singleton.transform;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector3 direction = _target.transform.position - transform.position;
            direction.Normalize();
            _moveDirection = _rotationAngle * Vector3.forward * _moveSpeed;

            _rigidbody.velocity = _moveDirection;

            ApplyRotation(direction);
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
