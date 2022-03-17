using Scripts.Managers.InGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers.InGame
{
    public class CollectableController : MonoBehaviour
    {
        [SerializeField] private Sprite _sickEggSprite;
        [SerializeField] private Sprite _healthyEggSprite;
        [SerializeField] private ParticleSystem _explosion;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _currentSprite;
        private CapsuleCollider2D _capsuleCollider2D;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _currentSprite = GetComponent<SpriteRenderer>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            ResetCollectable();
            _explosion.Pause();
        }

        public void MoveCollectable(int velocity)
        {
            _rigidbody.velocity = new Vector2(-velocity, _rigidbody.velocity.y);
            if((int)this.transform.localPosition.x <= -2000)
                ResetCollectable();
        }

        private void ResetCollectable()
        {
            this.transform.localPosition = new Vector3(
                2000,
                SetPositionY(),
                this.transform.localPosition.z);;
            _currentSprite.sprite = _sickEggSprite;
            _capsuleCollider2D.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _capsuleCollider2D.enabled = false;
            _explosion.Play();
            _currentSprite.sprite = _healthyEggSprite;
            UIManager.Instance.UpdateEggCount();
        }

        private int SetPositionY()
        {
            return (int)(UnityEngine.Random.Range(-3.25f, 0f) * 120);
        }
    } 
}
