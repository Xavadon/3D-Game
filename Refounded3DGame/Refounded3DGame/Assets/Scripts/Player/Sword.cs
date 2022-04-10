using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class Sword : MonoBehaviour
    {        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyStats enemy))
            {
                enemy.Hurt(GetComponentInParent<PlayerCombat>().Damage);
            }
        }
    }
}
