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
            Debug.Log(_rigidbody.velocity);
            UpdateAnimatorValues();
            GetPlayerFlags();
            if (!_playerHealth.IsDead) ApplyCameraRotation();

            HandleFalling();

            if (_isGrounded && _isInteracting && !_isRolling && !_playerFlags.isAttacking) StopMovement();
            if (_playerFlags.isAttacking) Roll();

            if (!_isGrounded || _isJumping || _isInteracting)
                return;

            Jump();
            Roll();
            Move();

        }

        private void UpdateAnimatorValues()
        {
            _animatorHandler.UpdateAnimatorValues(_rigidbody.velocity);
        }

        private void FixedUpdate()
        {

            if (!_isGrounded || _isJumping || _isInteracting)
                return; 
            
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
            if (!_isRolling)
            {
                SetMoveSpeed();
                _moveDirection = _rotationAngle * Vector3.forward * _moveSpeed * GetInput().magnitude;
                _rigidbody.velocity = _moveDirection;
            }
            else StopMovement();
        }

        private void ApplyCameraRotation()
        {
            Vector3 direction = GetInput();

            if (direction.magnitude > 0.1f)
            {
                float rotationAngle;
                float _targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;

                rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _rotationVelocity, SetRotationTime());

                _rotationAngle = Quaternion.Euler(0, rotationAngle, 0);
                transform.rotation = _rotationAngle;
            }
        }

        private float SetRotationTime()
        {
            if (_isRolling && _playerFlags.isAttacking)
                return _rotationTime;
            else if (_isRolling)
                return 10;
            else if (_playerFlags.isAttacking)
                return 0.5f;
            else if (_isInteracting)
                return 2;
            else
                return _rotationTime;
        }

        private void SetMoveSpeed()
        {
            if (Input.GetButton("Run") && !_isJumping)
                _moveSpeed = _defaultMoveSpeed * 1.7f;
            else if (_isInteracting)
                _moveSpeed = 0;
            else
                _moveSpeed = _defaultMoveSpeed;
        }

        private void Roll()
        {
            if (Input.GetButtonDown("Roll") && !_isRolling)
            {
                StartCoroutine(nameof(SetIsRolling));

                _animatorHandler.PlayTargetAnimation("Roll", true, 0.2f);
                Quaternion angle = Quaternion.Euler(0, Mathf.Atan2(GetInput().x, GetInput().z) * Mathf.Rad2Deg + _camera.eulerAngles.y, 0);

                if (GetInput() == Vector3.zero)
                    _rigidbody.AddForce(transform.forward * _rollForce, ForceMode.Impulse);
                else
                    _rigidbody.AddForce(angle * Vector3.forward * _rollForce, ForceMode.Impulse);
            }
        }

        private IEnumerator SetIsRolling()
        {
            _isRolling = true;
            _playerFlags.canTakeDamage = false;
            yield return new WaitForSeconds(0.6f);
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
