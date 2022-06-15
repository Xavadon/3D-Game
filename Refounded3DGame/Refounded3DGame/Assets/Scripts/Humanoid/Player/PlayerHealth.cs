using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OK
{
    [RequireComponent(typeof(PlayerFlags))]
    [RequireComponent(typeof(PlayerCharacteristics))]
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;

        private float _health;
        private float _maxHealth;
        private float _defence;

        private AnimatorHandler _animatorHandler;
        private PlayerFlags _playerFlags;
        private PlayerCharacteristics _playerCharacteristics;

        private bool _isDead = false;
        public bool IsDead => _isDead;
        public PlayerFlags PlayerFlags => _playerFlags;


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
            if (_health >= 0 && !_isDead && _playerFlags.canTakeDamage)
            {
                if (_playerFlags.isBlocking) 
                    damage -= damage * 0.8f;

                damage -= _defence;

                if (damage <= 0) 
                    damage = 1;

                _health -= damage;

                if (_health <= 0)
                {
                    _animatorHandler.PlayTargetAnimation("Death", true, 0.2f);
                    _isDead = true;
                }
                else if (_playerFlags.isBlocking) 
                    _animatorHandler.PlayTargetAnimation("BlockImpact", false, 0.1f);
                else
                    _animatorHandler.PlayTargetAnimation("Hurt", true, 0.1f);
            }
            FillHealthBar();
        }

        public void Heal(float heal)
        {
            _health += heal;
            if (_health > _maxHealth)
                _health = _maxHealth;
        }

        private void FillHealthBar()
        {
            _healthBar.fillAmount = _health / _maxHealth;
        }
    }
}
