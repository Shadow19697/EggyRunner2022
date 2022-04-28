using Scripts.Enums;
using Scripts.Managers.InGame;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers.InGame
{
    public class CollectableController : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _activeSprite; //0: SickEgg, 1: Life, 2: x2, 3: x3, 4: Immunity
        [SerializeField] private Sprite _healthyEggSprite;
        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private ParticleSystem _explosion;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _currentSprite;
        private CapsuleCollider2D _capsuleCollider2D;
        private int[] _randomPosY = new int[] { 0, -100, -200, -300};
        private CollectableTypeEnum _typeOfCollectable;
        private bool _isReady;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _currentSprite = GetComponent<SpriteRenderer>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            ResetCollectable();
            _explosion.Pause();
        }

        private void Update()
        {
            if ((int)this.transform.localPosition.x <= -1100)
                ResetCollectable();
        }

        public void MoveCollectable(float velocity)
        {
            _isReady = false;
            _rigidbody.velocity = new Vector2(-velocity, _rigidbody.velocity.y);
        }

        private void ResetCollectable()
        {
            this.transform.localPosition = new Vector3(
                1100,
                SetPositionY(),
                this.transform.localPosition.z);
            switch(UnityEngine.Random.Range(0, 5))
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
                    _typeOfCollectable = CollectableTypeEnum.Immunity;
                    break;
            }
            _capsuleCollider2D.enabled = true;
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
            _isReady = true;
        }

        private int SetPositionY()
        {
            return _randomPosY[UnityEngine.Random.Range(0,_randomPosY.Length)];
        }

        public bool IsReady()
        {
            return _isReady;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.CompareTag("Player"))
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
                        UIManager.Instance.DisplayImmunity();
                        break;
                } 
            }
        }
    } 
}
