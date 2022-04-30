using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyHealth))]
    public class EnemyCombat : MonoBehaviour
    {
        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _damage;

        public float Damage => _damage;

        private float _distanceToAttack;
        private Transform _target;
        private bool _isCooldown;
        private bool _isPlayerDead;

        private AnimatorHandler _animatorHandler;
        private EnemyHealth _enemyHealth;

        private void Start()
        {
            _animatorHandler = GetComponentInChildren<AnimatorHandler>();
            _enemyHealth = GetComponent<EnemyHealth>();
            _target = Player.singleton.transform;
            _distanceToAttack = GetComponent<EnemyMovement>().distanceToStop;
        }

        private void Update()
        {
            _isPlayerDead = Player.singleton.GetComponent<PlayerHealth>().IsDead;
            if (Vector3.Distance(_target.position, transform.position) < _distanceToAttack && !_isPlayerDead && !_isCooldown && !_enemyHealth.IsDead /*&& !_animatorHandler.isInteracting*/)
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
