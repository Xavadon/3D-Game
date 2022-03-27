using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerLocalmotion : MonoBehaviour
    {
        private Transform _cameraObject;
        private InputHandler _inputHandler;
        private Vector3 _moveDirection;

        [HideInInspector] public Transform _myTransform;
        [HideInInspector] public AnimatorHandler _animatorHandler;

        public /*new*/ Rigidbody _rigidbody;
        public GameObject _normalCamera;

        [Header("Stats")]
        [SerializeField] float _movementSpeed = 5;
        [SerializeField] float _rotationSpeed = 10;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _inputHandler = GetComponent<InputHandler>();
            _animatorHandler = GetComponentInChildren<AnimatorHandler>();
            _cameraObject = Camera.main.transform;
            _myTransform = transform;
            _animatorHandler.Initialize();
        }

        private void Update()
        {
            float delta = Time.deltaTime;

            _inputHandler.TickInput(delta);
            HandleMovement(delta);
            HandleRollingAndSprinting(delta);

        }

        #region Rotation
        private Vector3 _normalVector;
        private Vector3 _targetPosition;

        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = _inputHandler._moveAmount;

            targetDir = _cameraObject.forward * _inputHandler._vertical;
            targetDir += _cameraObject.right * _inputHandler._horizontal;
            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = _myTransform.forward;

            float rs = _rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(_myTransform.rotation, tr, rs * delta);

            _myTransform.rotation = targetRotation;
        }

        private void HandleMovement(float delta)
        {

            _moveDirection = _cameraObject.forward * _inputHandler._vertical;
            _moveDirection += _cameraObject.right * _inputHandler._horizontal;
            _moveDirection.Normalize();
            _moveDirection.y = 0;

            float speed = _movementSpeed;
            _moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, _normalVector);
            _rigidbody.velocity = projectedVelocity;

            _animatorHandler.UpdateAnimatorValues(0, _inputHandler._moveAmount);

            if (_animatorHandler._canRotate)
            {
                HandleRotation(delta);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (_animatorHandler._animator.GetBool("IsInteracting"))
                return;
            if (_inputHandler._rollFlag)
            {
                _moveDirection = _cameraObject.forward * _inputHandler._vertical;
                _moveDirection += _cameraObject.right * _inputHandler._horizontal;

                if (_inputHandler._moveAmount > 0)
                {
                    _animatorHandler.PlayTargetAnimation("Roll", true);
                    _moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(_moveDirection);
                    _myTransform.rotation = rollRotation;
                }
                else
                {
                    _animatorHandler.PlayTargetAnimation("Backstep", true);
                }
            }
        }

        #endregion
    }
}
