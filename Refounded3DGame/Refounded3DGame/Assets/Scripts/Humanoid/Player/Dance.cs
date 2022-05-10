using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class Dance : MonoBehaviour
    {
        [SerializeField] private AnimatorHandler _animatorHandler;
        [SerializeField] private GameObject[] _objectsToHide;

        private bool _isDancing = false;
        private void Update()
        {
            if (Input.GetButtonDown("Dance") && !_isDancing && _animatorHandler.animator.GetFloat("Horizontal") < 0.2f)
            {
                _isDancing = true;
                _animatorHandler.PlayTargetAnimation("Dance", false, 0.2f);
                SetObjectsActive(false);
            }

            if (_isDancing)
                if (_animatorHandler.animator.GetFloat("Horizontal") > 0.2f)
                {
                    _isDancing = false;
                    SetObjectsActive(true);
                }
        }

        private void SetObjectsActive(bool value)
        {
            for (int i = 0; i < _objectsToHide.Length; i++)
            {
                _objectsToHide[i].SetActive(value);
            }
        }
    }
}
