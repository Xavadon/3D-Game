using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    #region Movement
    [Header("Movement")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _battleWalkSpeed;
    [SerializeField] private float _runSpeed;
    private float _moveSpeed;
    private float _defaultMoveSpeed;
    private float _inputHorizontal => Input.GetAxisRaw("Horizontal");
    private float _inputVertical => Input.GetAxisRaw("Vertical");
    private Vector3 _lookDirection => new Vector3(_inputHorizontal, 0, _inputVertical).normalized;
    private Vector3 _moveDirection => Quaternion.Euler(0, _targetAngle, 0) * Vector3.forward;

    [SerializeField] private float _gravity;
    #endregion

    #region Combat
    [Header("Combat")]
    [SerializeField] private float _damage;
    public float Damage => _damage;

    [SerializeField] private GameObject _sword;
    private Collider _swordCollider;
    #endregion

    #region Rotation
    [Header("Rotation")]
    [SerializeField] private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;
    private float _targetAngle;
    #endregion

    #region Components
    [Header("Components")]
    [SerializeField] private Transform _camera;

    private Animator _animator;
    private CharacterController _characterController;
    #endregion

    #region States
    private bool _canAttack = true;
    private bool _canMove = true;
    private bool _canRotate = true;
    private bool _isAttacking;
    private bool _isFalling;
    private bool _battleMode;
    #endregion

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _swordCollider = _sword.GetComponent<Collider>();
    }


    //Кривой контроллер, посмотреть гайд, переписать.
    private void Update()
    {
        Move();
        Fall();
        Rotate(_lookDirection);
        SetMoveSpeed();
        UpdateAnimator();
        TakeSword();
        Attack();
        Dance();
    }

    private void TakeSword()
    {
        if (Input.GetButtonDown("TakeSword") && !_isFalling)
        {
            SetAllConditions(false);
            if (!_battleMode)
            {
                _battleMode = true;
                _sword.SetActive(true);

                _animator.SetTrigger("EnableBattleMode");
            }
            else
            {
                _battleMode = false;
                _sword.SetActive(false);

                _animator.SetTrigger("DisableBattleMode");
            }
            Invoke(nameof(TakeSwordEnd),0.2f);
        }
    }

    private void TakeSwordEnd()
    {
        SetAllConditions(true);
    }

    private void Attack()
    {
        if (Input.GetButton("Attack") && _canAttack && _battleMode && !_isFalling)
        {
            _isAttacking = true;
            SetAllConditions(false);
            _swordCollider.enabled = true;

            _animator.SetTrigger("Attack");
        }
    }

    private void CanRepeatAttack()
    {
        _swordCollider.enabled = false;
        _canAttack = true;
        _canRotate = true;
    }

    private void AttackEnd()
    {
        if (!_canAttack) return;
        Invoke(nameof(AfterAttackEnd), 0.3f);
    }

    private void AfterAttackEnd()
    {
        _isAttacking = false;
        _canMove = true;
    }

    private void SetAllConditions(bool value)
    {
        _canAttack = value;
        _canMove = value;
        _canRotate = value;
    }

    private void Move()
    {
        if (_lookDirection.magnitude > 0.1f && _canMove)
            _characterController.Move((_moveDirection.normalized * _moveSpeed + Vector3.down * _gravity) * Time.deltaTime);
        else
            _characterController.Move((Vector3.down * _gravity) * Time.deltaTime);
    }

    private void Fall()
    {
        if (!_isAttacking)
        {
            if (_characterController.velocity.y <= -6.9 && !_isFalling)
            {
                if (_isAttacking) return;
                _isFalling = true;
                _animator.SetTrigger("Fall");
            }
            else if (_characterController.velocity.y > -0.1f)
            {
                _isFalling = false;
            }
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
            _moveSpeed = _runSpeed;
        else if (_battleMode)
            _moveSpeed = _battleWalkSpeed;
        else    
            _moveSpeed = _walkSpeed;
    }

    private void Dance()
    {
        if (Input.GetButtonDown("Dance") && _lookDirection.magnitude < 0.1f)
        {
            _animator.SetTrigger("Dance");
            _animator.applyRootMotion = true;
            if(_battleMode) _battleMode = false;
            _sword.SetActive(false);
        }
        if(_lookDirection.magnitude > 0.1f)
            _animator.applyRootMotion = false;
    }

    private void UpdateAnimator()
    {
        float currentVelocity = new Vector2(_characterController.velocity.x, _characterController.velocity.z).magnitude;
        if (currentVelocity == 0)
            _animator.SetFloat("Velocity", 0);
        else
            _animator.SetFloat("Velocity", Mathf.Abs(_lookDirection.magnitude * _moveSpeed));

        _animator.SetBool("isFalling", _isFalling);
        _animator.SetBool("isAttacking", _isAttacking);
        _animator.SetBool("BattleMode", _battleMode);
    }
}