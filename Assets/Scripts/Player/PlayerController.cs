using Scripts.Managers.InGame;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject _rig;
        [SerializeField] private ParticleSystem _explosion;
        [SerializeField] private ParticleSystem _deathAnimation;
        [SerializeField] private AudioSource _deathSound;

        private CapsuleCollider2D _capsuleCollider2D;
        private Rigidbody2D _rigidbody2D;
        private bool _hasDied;

        private void Start()
        {
            PlayerAnimations.Rig = _rig;
            PlayerAnimations.Explosion = _explosion;
            PlayerAnimations.DeathAnimation = _deathAnimation;
            PlayerAnimations.InitAnimations();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _hasDied = false;
            _deathSound.Pause();
        }

        private void Update()
        {
            if (!UIManager.Instance.IsPlayerAlive() && !_hasDied)
            {
                _deathSound.Play();
                _capsuleCollider2D.enabled = false;
                _rig.SetActive(false);
                _rigidbody2D.bodyType = RigidbodyType2D.Static;
                _hasDied = true;
                PlayerAnimations.PlayDeathAnimation();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Obstacle"))
            {
                UIManager.Instance.UpdateLifesCount(-1);
                PlayerAnimations.PlayExposionAnimation();
            }
        }
    }
}