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
        private Vector3 _moveDirection;

        [Header("Ground")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _raycastOffset;
        [SerializeField] private float _raycastMaxDistance;
        [SerializeField] private float _fallingVelocity;
        [SerializeField] private float _leapingVelocity;
        private float _inAirTimer;

        private Vector3 groundRaycastOffst;

        [Header("Jump")]
        [SerializeField] private float _gravityIntencity;
        [SerializeField] private float _jumpHeight;


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
            GetPlayerFlags();
            HandleFalling();

            if (!_isGrounded || _isJumping || _isInteracting)
                return;

            Move();
            Jump();
        }

        private void GetPlayerFlags()
        {
            _isInteracting = _animatorHandler._isInteracting;
            _isJumping = _animatorHandler._isJumping;
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
            if (Input.GetButton("Run"))
                _moveSpeed = _defaultMoveSpeed * 1.7f;
            else
                _moveSpeed = _defaultMoveSpeed;
        }

        private void Jump()
        {
            if (Input.GetButton("Jump") && _isGrounded /*&& !_isInteracting*/)
            {
                float jumpingVelocity = Mathf.Sqrt(-2 * _gravityIntencity * _jumpHeight);
                Vector3 playerVelocity = _moveDirection;
                playerVelocity.y = jumpingVelocity;
                _rigidbody.velocity = playerVelocity;

                _animatorHandler._animator.SetBool("isJumping", true);
                _animatorHandler.PlayTargetAnimation("Jump", false);

            }
        }

        private void HandleFalling()
        {
            RaycastHit hit;
            groundRaycastOffst = transform.position;
            groundRaycastOffst.y += _raycastOffset;            

            if (!_isGrounded && !_isJumping)
            {
                if(!_isInteracting)
                    _animatorHandler.PlayTargetAnimation("Fall", true);

                _inAirTimer += Time.deltaTime;
                _rigidbody.AddForce(Vector3.down * _fallingVelocity * _inAirTimer);
                _rigidbody.AddForce(transform.forward * _leapingVelocity);
            }

            if (Physics.SphereCast(groundRaycastOffst, 0.2f, Vector3.down, out hit, _raycastMaxDistance, _groundLayer))
            {
                if (!_isGrounded && _isInteracting)
                {
                    _animatorHandler.PlayTargetAnimation("Land", true);
                }

                _isGrounded = true;
                _inAirTimer = 0;
            }
            else
            {
                _isGrounded = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundRaycastOffst + Vector3.down * _raycastMaxDistance, 0.2f);
        }
    }
}
