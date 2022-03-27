using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class AnimatorHandler : MonoBehaviour
    {
        public Animator _animator;
        public InputHandler _inputHandler;
        public PlayerLocalmotion _playerLocalmotion;
        private int _vertical;
        private int _horizontal;
        public bool _canRotate;

        public void Initialize()
        {
            _animator = GetComponent<Animator>();
            _inputHandler = GetComponentInParent<InputHandler>();
            _playerLocalmotion = GetComponentInParent<PlayerLocalmotion>();
            _vertical = Animator.StringToHash("Vertical");
            _horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
        {
            #region Vertical
            float vMovement = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                vMovement = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                vMovement = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                vMovement = -0.5f;
            }
            else if (verticalMovement< -0.55f)
            {
                vMovement = -1;
            }
            else
            {
                vMovement = 0;
            }
            #endregion

            #region Horizontal
            float hMovement = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                hMovement = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                hMovement = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                hMovement = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                hMovement = -1;
            }
            else
            {
                hMovement = 0;
            }
            #endregion

            _animator.SetFloat("Vertical", vMovement, 0.1f, Time.deltaTime);;
            _animator.SetFloat("Horizontal", hMovement, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            _animator.applyRootMotion = isInteracting;
            _animator.SetBool("IsInteracting", isInteracting);
            _animator.CrossFade(targetAnim, 0.2f);
        }

        public void SetRotate(bool value)
        {
            _canRotate = value;
        }

        private void OnAnimatorMove()
        {
            if (_inputHandler._isInteracting == false)
                return;

            float delta = Time.deltaTime;
            _playerLocalmotion._rigidbody.drag = 0;
            Vector3 deltaPosition = _animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition/ delta;
            _playerLocalmotion._rigidbody.velocity = velocity;
        }
    }
}
