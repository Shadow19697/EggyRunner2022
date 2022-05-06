using Scripts.Managers.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject _rig;
        [SerializeField] private ParticleSystem _explosion;
        [SerializeField] private ParticleSystem _deathAnimation;
        [SerializeField] private AudioSource _deathSound;
        [SerializeField] private GlowObjects _glowObjects;

        [System.Serializable]
        public class GlowObjects
        {
            public List<GameObject> _objects;
        }

        private static PlayerController _instance;
        public static PlayerController Instance { get { if (_instance == null) _instance = FindObjectOfType<PlayerController>(); return _instance; } }

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
            _glowObjects._objects.ForEach(obj => obj.SetActive(false));
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

        public IEnumerator GlowAnimation()
        {
            _glowObjects._objects.ForEach(obj => obj.SetActive(true));
            yield return new WaitForSeconds(11);
            _glowObjects._objects.ForEach(obj => obj.SetActive(false));
            yield return new WaitForSeconds(0.5f);
            _glowObjects._objects.ForEach(obj => obj.SetActive(true));
            yield return new WaitForSeconds(0.5f);
            _glowObjects._objects.ForEach(obj => obj.SetActive(false));
            yield return new WaitForSeconds(0.5f);
            _glowObjects._objects.ForEach(obj => obj.SetActive(true));
            yield return new WaitForSeconds(0.5f);
            _glowObjects._objects.ForEach(obj => obj.SetActive(false));
            yield return new WaitForSeconds(0.5f);
            _glowObjects._objects.ForEach(obj => obj.SetActive(true));
            yield return new WaitForSeconds(0.5f);
            _glowObjects._objects.ForEach(obj => obj.SetActive(false));
            yield return new WaitForSeconds(0.2f);
            SoundManager.Instance.PlayLevelMusic();
        }
    }
}