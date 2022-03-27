using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class PlayerManager : MonoBehaviour
    {
        private InputHandler _inputHandler;
        private Animator _animator;

        private void Start()
        {
            _inputHandler = GetComponent<InputHandler>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            _inputHandler._isInteracting = _animator.GetBool("IsInteracting");
            _inputHandler._rollFlag = false;
        }
    }
}
