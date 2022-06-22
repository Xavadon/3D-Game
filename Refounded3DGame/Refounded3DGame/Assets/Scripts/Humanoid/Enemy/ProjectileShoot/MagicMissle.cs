using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class MagicMissle : MonoBehaviour
    {
        private Transform _target => PlayerSingleton.singleton.transform;
        [SerializeField] private float _damage;
        [SerializeField] private float _speed;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _distancePredict;
        [SerializeField] private float _timeToDestroy;
        [SerializeField] private GameObject _explotion;

        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            transform.rotation = Quaternion.LookRotation((_target.position + Vector3.up) - transform.position);
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = transform.forward * _speed;
            if(Vector3.Distance(transform.position, _target.position) < _distancePredict && _speed > 0) Rotate();
        }

        private void Rotate()
        {
            var direction = (_target.position + Vector3.up) - transform.position;

            var rotation = Quaternion.LookRotation(direction);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerHealth player))
            {
                if (other.GetComponent<PlayerFlags>().canTakeDamage)
                {
                    Instantiate(_explotion, transform.position, Quaternion.identity);
                    player.Hurt(_damage);
                    _speed = 0;
                    StartCoroutine(nameof(DestroyAfterTime));
                }
            }
        }

        private IEnumerator DestroyAfterTime()
        {
            yield return new WaitForSeconds(_timeToDestroy);
            Destroy(gameObject);
        }
    }
}
