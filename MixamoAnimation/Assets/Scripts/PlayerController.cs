using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    #region Movement
    [SerializeField] private float _moveSpeed;
    private float _defaultMoveSpeed;
    private float _inputHorizontal => Input.GetAxisRaw("Horizontal");
    private float _inputVertical => Input.GetAxisRaw("Vertical");
    private Vector3 _lookDirection => new Vector3(_inputHorizontal, 0, _inputVertical).normalized;
    private Vector3 _moveDirection => Quaternion.Euler(0, _targetAngle, 0) * Vector3.forward;

    [SerializeField] private float _gravity;

    private bool _canMove = true;
    #endregion

    #region Combat
    private bool _canAttack = true;

    [SerializeField] private GameObject _sword;
    #endregion

    #region Rotation
    [SerializeField] private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;
    private float _targetAngle;
    private bool _canRotate = true;
    #endregion

    #region Components
    [SerializeField] private Transform _camera;

    private Animator _animator;
    private CharacterController _characterController;
    #endregion

    private void Start()
    {
        _defaultMoveSpeed = _moveSpeed;

        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Attack();
        Move();
        Rotate(_lookDirection);
        SetMoveSpeed();
        UpdateAnimator();
    }

    private void Attack()
    {
        if (Input.GetButton("Attack") && _canAttack)
        {
            _canAttack = false;
            _canMove = false;
            _canRotate = false;
            _sword.SetActive(true);

            _animator.SetTrigger("Attack");
        }
    }

    private void CanRepeatAttack()
    {
        _canAttack = true;
        _canRotate = true;
    }

    private void AttackEnd()
    {
        if (!_canAttack) return;
        _canMove = true;
        Invoke(nameof(DisableSword), 1);
    }

    private void DisableSword()
    {
        if (!_canAttack) return;
        else
            _sword.SetActive(false);
    }

    private void Move()
    {
        if(_lookDirection.magnitude > 0.1f && _canMove)
        {
            _characterController.Move((_moveDirection.normalized * _moveSpeed + Vector3.down * _gravity) * Time.deltaTime);
        }
        else
        {
            _characterController.Move((Vector3.down * _gravity) * Time.deltaTime);
        }
    }

    private void Rotate(Vector3 direction)
    {
        if (_canRotate && _lookDirection.magnitude > 0.1f)
        {
            _targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

    private void SetMoveSpeed()
    {
        if (Input.GetButton("Charge"))
            _moveSpeed = _defaultMoveSpeed * 3f;
        else
            _moveSpeed = _defaultMoveSpeed;
    }

    private void UpdateAnimator()
    {
        float currentVelocity = new Vector2(_characterController.velocity.x, _characterController.velocity.z).magnitude;
        if (currentVelocity == 0)
            _animator.SetFloat("Velocity", 0);
        else
            _animator.SetFloat("Velocity", Mathf.Abs(_lookDirection.magnitude * _moveSpeed));
    }

}