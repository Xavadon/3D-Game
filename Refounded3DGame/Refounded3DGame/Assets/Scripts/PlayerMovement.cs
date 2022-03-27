using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed;
        private float _defaultMoveSpeed;

        [Header("Ground")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _raycastOffset;
        [SerializeField] private float _fallingVelocity;
        [SerializeField] private float _leapingVelocity;
        private float _inAirTimer;

        [Header("Player Flags")]
        [SerializeField] private bool _isGrounded = true;

        [Header("Rotation")]
        [SerializeField] private float _rotationTime;
        private float _rotationVelocity;
        private Quaternion _rotationAngle;

        [Header("Components")]
        [SerializeField] private Transform _camera;

        private AnimatorHandler _animatorHandler;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _defaultMoveSpeed = _moveSpeed;

            _animatorHandler = GetComponentInChildren<AnimatorHandler>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            HandleFalling();

            if (!_isGrounded)
                return;

            Move();
        }

        private void Move()
        {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            input.Normalize();

            _rigidbody.velocity = _rotationAngle * Vector3.forward * _moveSpeed * input.magnitude;

            ApplyCameraRotation(input);
            SetMoveSpeed();
            _animatorHandler.UpdateAnimatorValues(_rigidbody.velocity);
        }

        private void ApplyCameraRotation(Vector3 direction)
        {
            float _targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _rotationVelocity, _rotationTime);
            _rotationAngle = Quaternion.Euler(0, rotationAngle, 0);
            transform.rotation = _rotationAngle;
        }

        private void SetMoveSpeed()
        {
            if (Input.GetButton("Run"))
                _moveSpeed = _defaultMoveSpeed * 1.7f;
            else
                _moveSpeed = _defaultMoveSpeed;
        }

        private void HandleFalling()
        {
            RaycastHit hit;
            Vector3 groundRaycastOffst = transform.position;
            groundRaycastOffst.y += _raycastOffset;            

            if (!_isGrounded)
            {
                _animatorHandler.PlayTargetAnimation("Fall");

                _inAirTimer += Time.deltaTime;
                _rigidbody.AddForce(Vector3.down * _fallingVelocity * _inAirTimer);
                _rigidbody.AddForce(transform.forward * _leapingVelocity);
            }

            if (Physics.SphereCast(groundRaycastOffst, 0.2f, Vector3.down, out hit, _groundLayer))
            {
                if (!_isGrounded)
                {
                    _animatorHandler.PlayTargetAnimation("Land");
                }

                _isGrounded = true;
                _inAirTimer = 0;
            }
            else
            {
                _isGrounded = false;
            }
        }
    }
}
