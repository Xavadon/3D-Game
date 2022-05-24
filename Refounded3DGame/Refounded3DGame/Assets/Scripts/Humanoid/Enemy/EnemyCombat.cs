using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(EnemyFlags))]
    public class EnemyCombat : MonoBehaviour
    {
        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _attackDistance;
        [SerializeField] private float _damage;

        public float Damage => _damage;

        private Transform _target;
        private bool _isCooldown;
        private bool _isPlayerDead;

        private AnimatorHandler _animatorHandler;
        private EnemyHealth _enemyHealth;
        private EnemyFlags _enemyFlags;

        private void Start()
        {
            _enemyHealth = GetComponent<EnemyHealth>();
            _enemyFlags = GetComponent<EnemyFlags>();
            _animatorHandler = _enemyFlags.animatorHandler;
            _target = PlayerSingleton.singleton.transform;
        }

        private void Update()
        {
            _isPlayerDead = PlayerSingleton.singleton.GetComponent<PlayerHealth>().IsDead;
            if (Vector3.Distance(_target.position, transform.position) < _attackDistance 
                && !_isPlayerDead && !_isCooldown && !_enemyHealth.IsDead && !_enemyFlags.isInteracting)
            {
                Attack();
            }
        }

        private void Attack()
        {
            _animatorHandler.PlayTargetAnimation("Attack", true, 0.1f);

            StartCoroutine(nameof(Cooldown));
        }

        private IEnumerator Cooldown()
        {
            _isCooldown = true;
            yield return new WaitForSeconds(_attackCooldown);
            _isCooldown = false;
        }
    }
}
