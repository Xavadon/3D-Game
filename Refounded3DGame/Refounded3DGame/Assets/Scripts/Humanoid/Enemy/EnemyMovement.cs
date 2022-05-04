using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyFlags))]
    public class EnemyMovement : MonoBehaviour
    {
        private Transform _target;

        [Header("Distances")]
        [SerializeField] private float _chasingDistance;
        public float stoppingDistance;

        private EnemyHealth _enemyHealth;
        private NavMeshAgent _navMeshAgent;

        private AnimatorHandler _animatorHandler;

        private void Start()
        {
            _enemyHealth = GetComponent<EnemyHealth>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animatorHandler = GetComponent<EnemyFlags>().animatorHandler;
            _target = PlayerSingleton.singleton.transform;
        }

        private void Update()
        {
            if (_enemyHealth.IsDead)
                return;

            Move();
        }

        private void Move()
        {
            Vector3 direction = _target.transform.position - transform.position;
            direction.Normalize();

            if (Vector3.Distance(_target.position, transform.position) < _chasingDistance
                && Vector3.Distance(_target.position, transform.position) > stoppingDistance)
            {
                _navMeshAgent.SetDestination(_target.position);
                _animatorHandler.UpdateAnimatorValues(new Vector3(1, 0, 0));
            }
            else
            {
                _navMeshAgent.SetDestination(transform.position);
                _animatorHandler.UpdateAnimatorValues(Vector3.zero);
            }
        }
    }
}
