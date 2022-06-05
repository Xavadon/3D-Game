using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(PlayerFlags))]
    [RequireComponent(typeof(PlayerCharacteristics))]
    public class PlayerHealth : MonoBehaviour
    {
        private float _health;
        private float _maxHealth;
        private float _defence;

        private AnimatorHandler _animatorHandler;
        private PlayerFlags _playerFlags;
        private PlayerCharacteristics _playerCharacteristics;

        private bool _isDead = false;
        public bool IsDead => _isDead;


        private void Start()
        {
            _playerCharacteristics = GetComponent<PlayerCharacteristics>();
            _health = _playerCharacteristics.Health;
            _maxHealth = _health;
            _defence = _playerCharacteristics.Defence;

            _playerFlags = GetComponent<PlayerFlags>();
            _animatorHandler = _playerFlags.animatorHandler;
        }

        public void Hurt(float damage)
        {
            if (_health > 0 && !_isDead && _playerFlags.canTakeDamage)
            {
                damage -= _defence;
                _health -= damage;

                if (_health <= 0)
                {
                    _animatorHandler.PlayTargetAnimation("Death", true, 0.2f);
                    _isDead = true;
                }
                else
                    _animatorHandler.PlayTargetAnimation("Hurt", true, 0.1f);

            }
        }

        public void Heal(float heal)
        {
            _health += heal;
            if (_health > _maxHealth)
                _health = _maxHealth;
        }
    }
}
