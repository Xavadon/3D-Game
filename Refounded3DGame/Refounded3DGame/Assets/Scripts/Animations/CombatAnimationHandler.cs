using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class CombatAnimationHandler : MonoBehaviour
    {
        [SerializeField] private Collider _damageCollider;

        public void EnableDamageCollider()
        {
            _damageCollider.enabled = true;
        }
        
        public void DisableDamageCollider()
        {
            _damageCollider.enabled = false;
        }

        public void FootStep()
        {
            //nothing
        }
    }
}
