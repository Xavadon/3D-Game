using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class PlayerCombatAnimations : MonoBehaviour
    {
        [SerializeField] private Collider _damageCollider;
        [SerializeField] private GameObject[] _eqipmetToHide;

        public void EnableDamageCollider()
        {
            _damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            _damageCollider.enabled = false;
        }

        public void HideEqipment()
        {
            for (int i = 0; i < _eqipmetToHide.Length; i++)
            {
                _eqipmetToHide[i].SetActive(false);
            }
        }
        public void ShowEqipment()
        {
            for (int i = 0; i < _eqipmetToHide.Length; i++)
            {
                _eqipmetToHide[i].SetActive(true);
            }
        }
    }
}
