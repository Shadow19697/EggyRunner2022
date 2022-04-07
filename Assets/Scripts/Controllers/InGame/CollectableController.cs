using Scripts.Enums;
using Scripts.Managers.InGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers.InGame
{
    public class CollectableController : MonoBehaviour
    {
        [SerializeField] private GameObject _this;
        [SerializeField] private List<Sprite> _activeSprite; //0: SickEgg, 1: Life, 2: x2, 3: x3, 4: Immortality
        [SerializeField] private Sprite _healthyEggSprite;
        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private ParticleSystem _explosion;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _currentSprite;
        private CapsuleCollider2D _capsuleCollider2D;
        private int[] _randomPosY = new int[] { 0, -100, -200, -300, -390 };
        private CollectableTypeEnum _typeOfCollectable;

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
                this.transform.localPosition.z);
            switch(UnityEngine.Random.Range(0, 4))
            {
                case 0: _currentSprite.sprite = _activeSprite[0];
                    _typeOfCollectable = CollectableTypeEnum.SickEgg;
                    break;
                case 1: _currentSprite.sprite = _activeSprite[1];
                    _typeOfCollectable = CollectableTypeEnum.Life;
                    break;
                case 2:
                    _currentSprite.sprite = _activeSprite[2];
                    _typeOfCollectable = CollectableTypeEnum.x2;
                    break;
                case 3:
                    _currentSprite.sprite = _activeSprite[3];
                    _typeOfCollectable = CollectableTypeEnum.x3;
                    break;
                default:
                    _currentSprite.sprite = _activeSprite[4];
                    _typeOfCollectable = CollectableTypeEnum.Immortality;
                    break;
            }
            _capsuleCollider2D.enabled = true;
        }

        private int SetPositionY()
        {
            return _randomPosY[UnityEngine.Random.Range(0,4)];
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _capsuleCollider2D.enabled = false;
            _explosion.Play();
            _currentSprite.sprite = _emptySprite;
            switch (_typeOfCollectable)
            {
                case CollectableTypeEnum.SickEgg:
                    _currentSprite.sprite = _healthyEggSprite;
                    UIManager.Instance.UpdateEggCount();
                    break;
                case CollectableTypeEnum.Life:
                    UIManager.Instance.UpdateLifesCount(1);
                    break;
                case CollectableTypeEnum.x2:
                    UIManager.Instance.UpdateScoreMultiplier(2);
                    break;
                case CollectableTypeEnum.x3:
                    UIManager.Instance.UpdateScoreMultiplier(3);
                    break;
                default:
                    ObjectsManager.Instance.DisableObstacleCollider();
                    break;
            }
        }
    } 
}
