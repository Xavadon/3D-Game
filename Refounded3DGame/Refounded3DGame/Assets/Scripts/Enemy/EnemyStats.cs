﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class EnemyStats : MonoBehaviour
    {
        [SerializeField] private float _health;
        private float _maxHealth;

        private AnimatorHandler _animatorHandler;

        private bool _isDead = false;
        public  bool IsDead => _isDead;

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
                {
                    _animatorHandler.PlayTargetAnimation("Death", true, 0.2f);
                    _isDead = true;
                }

                else
                    _animatorHandler.PlayTargetAnimation("Hurt", true, 0.2f);

            }
        }
    }
}