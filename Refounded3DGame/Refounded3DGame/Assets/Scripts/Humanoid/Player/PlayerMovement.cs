using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(PlayerFlags))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerHealth))]
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

        [Header("Roll")]
        [SerializeField] private float _rollForce;

        [Header("Player Flags")]
        [SerializeField] private bool _isGrounded = true;
        private bool _isInteracting;
        private bool _isJumping;
        private bool _isRolling;


        [Header("Rotation")]
        [SerializeField] private float _rotationTime;
        private float _rotationVelocity;
        private Quaternion _rotationAngle;

        [Header("Components")]
        [SerializeField] private Transform _camera;

        private PlayerFlags _playerFlags;
        private AnimatorHandler _animatorHandler;
        private Rigidbody _rigidbody;
        private PlayerHealth _playerHealth;

        private void Start()
        {
            _defaultMoveSpeed = _moveSpeed;

            _playerFlags = GetComponent<PlayerFlags>();
            _animatorHandler = _playerFlags.animatorHandler;
            _rigidbody = GetComponent<Rigidbody>();
            _playerHealth = GetComponent<PlayerHealth>();
        }

        private void Update()
        {
            GetPlayerFlags();
            if (!_playerHealth.IsDead) ApplyCameraRotation();

            HandleFalling();

            if (_isGrounded && _isInteracting && !_isRolling) StopMovement();

            if (!_isGrounded || _isJumping || _isInteracting)
                return;

            Move();
            Jump();
            Roll();

        }

        private void GetPlayerFlags()
        {
            _isInteracting = _playerFlags.isInteracting;
            _isJumping = _playerFlags.isJumping;

        }

        private Vector3 GetInput()
        {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            input.Normalize();
            return input;
        }

        private void StopMovement()
        {
            _rigidbody.velocity = Vector3.zero;
        }

        private void Move()
        {
            _moveDirection = _rotationAngle * Vector3.forward * _moveSpeed * GetInput().magnitude;

            _rigidbody.velocity = _moveDirection;

            SetMoveSpeed();
            _animatorHandler.UpdateAnimatorValues(_rigidbody.velocity);
        }

        private void ApplyCameraRotation()
        {
            Vector3 direction = GetInput();

            if (direction.magnitude > 0.1f)
            {
                float rotationAngle;
                float _targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;

                if(_isRolling || _isInteracting)
                    rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _rotationVelocity, 2);
                else
                    rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _rotationVelocity, _rotationTime);

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

        private void Roll()
        {
            if (Input.GetButtonDown("Roll") && !_isRolling)
            {
                StopMovement();
                _animatorHandler.PlayTargetAnimation("Roll", true, 0.2f);
                Quaternion angle = Quaternion.Euler(0, Mathf.Atan2(GetInput().x, GetInput().z) * Mathf.Rad2Deg + _camera.eulerAngles.y, 0);
               
                if(GetInput() == Vector3.zero)
                    _rigidbody.AddForce(transform.forward * _rollForce, ForceMode.Impulse);
                else
                    _rigidbody.AddForce(angle * Vector3.forward * _rollForce, ForceMode.Impulse);

                StartCoroutine(nameof(SetIsRolling));
            }
        }

        private IEnumerator SetIsRolling()
        {
            _isRolling = true;
            _playerFlags.canTakeDamage = false;
            yield return new WaitForSeconds(0.7f);
            _isRolling = false;
            _playerFlags.canTakeDamage = true;
        }

        private void Jump()
        {
            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

                _animatorHandler.PlayTargetAnimation("Jump", false, 0.1f);
                StartCoroutine(nameof(SetIsJumping));
            }
        }

        private IEnumerator SetIsJumping()
        {
            _animatorHandler.animator.SetBool("isJumping", true);
            yield return new WaitForSeconds(0.3f);
            _animatorHandler.animator.SetBool("isJumping", false);
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
                    _animatorHandler.PlayTargetAnimation("Fall", true, 0.1f);
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
