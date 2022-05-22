using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class NecromancerAttackHandler : MonoBehaviour
    {
        [SerializeField] private Transform _projectileSpawnPoint;
        [SerializeField] private GameObject _projectile;
        [SerializeField] private Transform[] _minionsSpawnPoints;
         private GameObject[] _minions = new GameObject[3];
        [SerializeField] private GameObject _minionPrefab;
        private bool _canSpawnMinions = true;

        public void NecromancerAttack()
        {
            if (_canSpawnMinions) SpawnMinions();
            else ShootProjectile();
        }

        private void ShootProjectile()
        {
            Instantiate(_projectile, _projectileSpawnPoint.position, Quaternion.identity);
        }

        private void SpawnMinions()
        {
            if (_canSpawnMinions)
            {
                _canSpawnMinions = false;

                for (int i = 0; i < _minionsSpawnPoints.Length; i++)
                {
                    _minions[i] = Instantiate(_minionPrefab, _minionsSpawnPoints[i].position, Quaternion.identity);
                }
            }
        }
    }
}
