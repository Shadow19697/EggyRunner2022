using Scripts.Managers.InGame;
using UnityEngine;

namespace Scripts.Controllers.InGame
{
    public class ObstacleController : MonoBehaviour
    {
        [SerializeField] private CapsuleCollider2D _damageCollider2D;
        [SerializeField] private bool _isGreen;
        [SerializeField] private AudioSource _cryingCovidSound;
        
        private CapsuleCollider2D _capsuleCollider2D;
        private Transform _transform;
        private Rigidbody2D _rigidbody;
        private bool _isReady;
        private bool _launchAnother;
        private int[] _randomPosY = new int[] { -200, -300 };
        private Vector3 _obstacleScale;

        private void Start()
        {
            if (_isGreen)
            {
                _cryingCovidSound.Pause();
                _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            }
            _transform = GetComponent<Transform>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _launchAnother = false;
            _obstacleScale = _transform.localScale;
            ResetObstacle();
        }

        private void Update()
        {
            if ((int)this.transform.localPosition.x <= - (UnityEngine.Random.Range(200, 500)))
                _launchAnother = true;
            if ((int)this.transform.localPosition.x <= -1200)
                ResetObstacle();
        }

        private void ResetObstacle()
        {
            this.transform.localPosition = new Vector3(
                1200,
                SetPositionY(),
                this.transform.localPosition.z);
            _transform.localScale = _obstacleScale;
            if (_isGreen) _capsuleCollider2D.enabled = true;
            if (UIManager.Instance.IsImmunityActivated()) _damageCollider2D.enabled = false;
            else _damageCollider2D.enabled = true;
            _rigidbody.velocity = new Vector2(0, 0);
            _isReady = true;
        }

        private int SetPositionY()
        {
            return _randomPosY[UnityEngine.Random.Range(0, _randomPosY.Length)];
        }

        public bool IsReady()
        {
            return _isReady;
        }

        public bool LaunchAnother()
        {
            return _launchAnother;
        }

        public void MoveObstacle(float velocity)
        {
            _isReady = false;
            _launchAnother = false;
            _rigidbody.velocity = new Vector2(-velocity, _rigidbody.velocity.y);
        }

        public CapsuleCollider2D GetDamageCollider()
        {
            return _damageCollider2D;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _cryingCovidSound.Play();
                _transform.localScale = new Vector3(_obstacleScale.x, _obstacleScale.y * 0.3f, _obstacleScale.z);
                _capsuleCollider2D.enabled = false;
                _damageCollider2D.enabled = false;
                _rigidbody.AddForce(Vector2.down * 20);
                UIManager.Instance.AddObstacleCount();
            }
        }
    }
}
