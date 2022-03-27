﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class InputHandler : MonoBehaviour
    {
        public float _horizontal;
        public float _vertical;
        public float _moveAmount;
        public float _mouseX;
        public float _mouseY;

        public bool _b_Input;

        public bool _isInteracting;

        public bool _rollFlag;

        private PlayerControls _inputActions;
        private CameraHandler _cameraHandler;

        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        private void Awake()
        {
            _cameraHandler = CameraHandler._singleton;
        }

        private void FixedUpdate()
        {
            float delta = Time.deltaTime;

            if(_cameraHandler != null)
            {
                _cameraHandler.FollowTarget(delta);
            }
        }

        private void Update()
        {
            if (_cameraHandler != null)
            {
                _cameraHandler.HandleCameraRotation( _mouseX, _mouseY);
            }
        }

        public void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new PlayerControls();
                _inputActions.PlayerMovement.Movement.performed += _inputActions => _movementInput = _inputActions.ReadValue<Vector2>();
                _inputActions.PlayerMovement.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();
            }

            _inputActions.Enable();
        }

        public void OnDisable()
        {
            _inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
        }

        private void MoveInput(float delta)
        {
            _horizontal = _movementInput.x;
            _vertical = _movementInput.y;
            _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));
            _mouseX = _cameraInput.x;
            _mouseY = _cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            _b_Input = _inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if (_b_Input)
            {
                _rollFlag = true;
            }
        }
    }
}
