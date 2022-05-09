using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class Damager : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerHealth player))
            {
                player.Hurt(GetComponentInParent<EnemyCombat>().Damage);
            }
        }
    }
}
