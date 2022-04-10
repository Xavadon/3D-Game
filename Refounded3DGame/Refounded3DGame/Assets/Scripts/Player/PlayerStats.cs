using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private float _health;
        private float _maxHealth;

        private AnimatorHandler _animatorHandler;

        public static PlayerStats _singleton { get; private set; }

        private void Awake()
        {
            _singleton = this;
        }

        private void Start()
        {
            _maxHealth = _health;
            _animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }


        public void Hurt(float damage)
        {
            if (_health > 0)
            {
                _health -= damage;

                if (_health <= 0)
                    _animatorHandler.PlayTargetAnimation("Death", true, 0.2f);
                else
                    _animatorHandler.PlayTargetAnimation("Hurt", true, 0.2f);

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
