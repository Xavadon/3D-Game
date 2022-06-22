using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class MageAttackAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _projectile;
        public void ShootProjectile()
        {
            Instantiate(_projectile, _spawnPoint.position, Quaternion.identity);
        }
    }
}
