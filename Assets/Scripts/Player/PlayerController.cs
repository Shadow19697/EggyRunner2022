using Scripts.Managers.InGame;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject _rig;
        [SerializeField] private ParticleSystem _explosion;
        [SerializeField] private ParticleSystem _deathAnimation;

        private CapsuleCollider2D _capsuleCollider2D;
        private Rigidbody2D _rigidbody2D;

        private void Start()
        {
            PlayerAnimations.Rig = _rig;
            PlayerAnimations.Explosion = _explosion;
            PlayerAnimations.DeathAnimation = _deathAnimation;
            PlayerAnimations.InitAnimations();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (UIManager.Instance.GetLifesCount() == 0)
            {
                _capsuleCollider2D.enabled = false;
                _rig.SetActive(false);
                _rigidbody2D.bodyType = RigidbodyType2D.Static;
                PlayerAnimations.PlayDeathAnimation();
            }
        }
    }
}