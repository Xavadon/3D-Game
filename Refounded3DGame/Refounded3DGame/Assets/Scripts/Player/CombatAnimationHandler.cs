using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class CombatAnimationHandler : MonoBehaviour
    {
        [SerializeField] private Collider _swordCollider;

        public void EnableSwordCollider()
        {
            _swordCollider.enabled = true;
        }
        
        public void DisableSwordCollider()
        {
            _swordCollider.enabled = false;
        }

        public void FootStep()
        {
            //nothing
        }
    }
}
