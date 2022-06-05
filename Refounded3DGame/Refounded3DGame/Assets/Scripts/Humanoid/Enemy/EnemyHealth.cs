using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(EnemyFlags))]
    [RequireComponent(typeof(EnemyCollisionHandler))]
    [RequireComponent(typeof(EnemyMovement))]
    public class EnemyHealth : MonoBehaviour
    {
        private EnemyCollisionHandler _collisionHandler;
        [SerializeField] private float _health;
        [SerializeField] private float _expGain;
        private float _maxHealth;

        private AnimatorHandler _animatorHandler;

        private bool _isDead = false;
        public  bool IsDead => _isDead;

        private void Start()
        {
            _maxHealth = _health;
            _animatorHandler = GetComponent<EnemyFlags>().animatorHandler;
            _collisionHandler = GetComponent<EnemyCollisionHandler>();
        }

        public void Hurt(float damage)
        {
            if (_health > 0)
            {
                _health -= damage;

                if (_health <= 0)
                {
                    Death();
                }
                else
                {
                    _animatorHandler.PlayTargetAnimation("Hurt", true, 0.05f);
                }
            }
        }

        private void Death()
        {
            _animatorHandler.PlayTargetAnimation("Death", true, 0.2f);
            _isDead = true;
            _collisionHandler.DisableCollisions();
            GetComponent<EnemyMovement>()._target.GetComponent<PlayerCharacteristics>().AddExp(_expGain);
        }
    }
}