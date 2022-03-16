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

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _currentSprite;
        private BoxCollider2D _boxCollider2D;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _currentSprite = GetComponent<SpriteRenderer>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            ResetCollectable();
        }

        public void MoveCollectable(int velocity)
        {
            _rigidbody.velocity = new Vector2(-velocity, _rigidbody.velocity.y);
            if((int)this.transform.localPosition.x <= -2000)
            {
                ResetCollectable();
            }
        }

        private void ResetCollectable()
        {
            this.transform.localPosition = new Vector3(
                2000,
                this.transform.localPosition.y,
                this.transform.localPosition.z);
            _currentSprite.sprite = _sickEggSprite;
            _boxCollider2D.enabled = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _boxCollider2D.enabled = false;
            _currentSprite.sprite = _healthyEggSprite;
            UIManager.Instance.UpdateEggCount();
        }
    } 
}
