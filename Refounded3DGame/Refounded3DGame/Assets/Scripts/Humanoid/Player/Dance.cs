using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class Dance : MonoBehaviour
    {
        [SerializeField] private AnimatorHandler _animatorHandler;
        private void Update()
        {
            if (Input.GetButtonDown("Dance"))
            {
                _animatorHandler.PlayTargetAnimation("Dance", false, 0.2f);
            }
        }
    }
}
