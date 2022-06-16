using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class SoundEffects: MonoBehaviour
    {
        [SerializeField] private AudioList _sound;
        [SerializeField] private AudioSource _audioSource;

        private void FootStep()
        {
            if (_sound.footsteps[0])
            {
                _audioSource.clip = _sound.footsteps[Random.Range(0, _sound.footsteps.Length)];
                _audioSource.Play();
            }
        }

        private void Jump()
        {
            if (_sound.jump[0])
            {
                _audioSource.clip = _sound.jump[Random.Range(0, _sound.jump.Length)];
                _audioSource.Play();
            }
        }

        private void Hurt()
        {
            if (_sound.hurt[0])
            {
                _audioSource.clip = _sound.hurt[Random.Range(0, _sound.hurt.Length)];
                _audioSource.Play();
            }
        }
        private void Death()
        {
            if (_sound.dead[0])
            {
                _audioSource.clip = _sound.dead[Random.Range(0, _sound.dead.Length)];
                _audioSource.Play();
            }
        }

        private void SwordSwing()
        {
            if (_sound.swordSwings[0])
            {
                _audioSource.clip = _sound.swordSwings[Random.Range(0, _sound.swordSwings.Length)];
                _audioSource.Play();
            }
        }
    }
}
