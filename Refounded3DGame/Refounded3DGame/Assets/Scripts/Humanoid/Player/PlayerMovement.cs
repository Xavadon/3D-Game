using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(PlayerFlags))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed;
        private float _defaultMoveSpeed;
        private Vector3 _moveDirection;

        [Header("Ground")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _raycastOffset;
        [SerializeField] private float _raycastMaxDistance;
        [SerializeField] private float _fallingVelocity;
        private Vector3 rayCastHitPoint;
        private bool _isFalling;

        [Header("Jump")]
        [SerializeField] private float _jumpForce;


        [Header("Player Flags")]
        [SerializeField] private bool _isGrounded = true;
        private bool _isInteracting;
        private bool _isJumping;


        [Header("Rotation")]
        [SerializeField] private float _rotationTime;
        private float _rotationVelocity;
        private Quaternion _rotationAngle;

        [Header("Components")]
        [SerializeField] private Transform _camera;

        [SerializeField] private AnimatorHandler _animatorHandler;
        private PlayerFlags _playerFlags;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _defaultMoveSpeed = _moveSpeed;

            _playerFlags = GetComponent<PlayerFlags>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            GetPlayerFlags();
            HandleFalling();

            if (_isGrounded && _isInteracting)
                StopMovement();

            if (!_isGrounded || _isJumping || _isInteracting)
                return;

            Move();
            Jump();
        }

        private void GetPlayerFlags()
        {
            _isInteracting = _playerFlags.isInteracting;
            _isJumping = _playerFlags.isJumping;

        }

        private void StopMovement()
        {
            _rigidbody.velocity = Vector3.zero;
        }

        private void Move()
        {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            input.Normalize();
            _moveDirection = _rotationAngle * Vector3.forward * _moveSpeed * input.magnitude;

            _rigidbody.velocity = _moveDirection;

            ApplyCameraRotation(input);
            SetMoveSpeed();
            _animatorHandler.UpdateAnimatorValues(_rigidbody.velocity);
        }

        private void ApplyCameraRotation(Vector3 direction)
        {
            if (direction.magnitude > 0.1f)
            {
                float _targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
                float rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _rotationVelocity, _rotationTime);
                _rotationAngle = Quaternion.Euler(0, rotationAngle, 0);
                transform.rotation = _rotationAngle;
            }
        }

        private void SetMoveSpeed()
        {
            if (Input.GetButton("Run") && !_isJumping)
                _moveSpeed = _defaultMoveSpeed * 1.7f;
            else
                _moveSpeed = _defaultMoveSpeed;
        }

        private void Jump()
        {
            if (Input.GetButton("Jump") && _isGrounded)
            {

                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

                _animatorHandler.animator.SetBool("isJumping", true);
                _animatorHandler.PlayTargetAnimation("Jump", false, 0.1f);
            }
        }

        private void HandleFalling()
        {
            Vector3 position = transform.position;
            RaycastHit hit;
            Vector3 groundRaycastOffst = transform.position;
            groundRaycastOffst.y += _raycastOffset;

            _animatorHandler.animator.SetBool("isGrounded", _isGrounded);

            if (!_isGrounded && !_isJumping)
            {
                if (!_isInteracting)
                {
                    _animatorHandler.PlayTargetAnimation("Fall", false, 0.1f);
                    _isFalling = true;
                }

                _rigidbody.velocity += Vector3.down * _fallingVelocity * Time.deltaTime;
            }

            if (Physics.SphereCast(groundRaycastOffst, 0.01f, -Vector3.up, out hit, _raycastMaxDistance, _groundLayer))
            {
                rayCastHitPoint = hit.point;
                position.y = rayCastHitPoint.y;

                _isGrounded = true;

                if (_isGrounded && _isFalling)
                {
                    _isFalling = false;
                    _animatorHandler.PlayTargetAnimation("Land", false, 0.1f);
                    transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime / 0.2f);
                }

                if (!_isJumping)
                {
                    transform.position = position;
                }
            }
            else
            {
                _isGrounded = false;
            }

        }
    }
}
