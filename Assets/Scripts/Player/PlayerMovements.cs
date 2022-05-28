using Scripts.Managers;
using Scripts.Managers.InGame;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerMovements : MonoBehaviour
    {
        [SerializeField] private int _jumpForce;
        [SerializeField] private AudioSource _jumpSound;

        private Rigidbody2D _rigidbody2D;
        private float _inputVertical;
        private bool _onGround = false;
        private bool _isSpaceLevel;


        public void Start()
        {
            _jumpSound.Pause();
            if (PlayerPrefsManager.GetLevelSelected() == 3)
                _isSpaceLevel = true;
            else _isSpaceLevel = false;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            if (_isSpaceLevel)
            {
                _rigidbody2D.gravityScale = 1;
                _rigidbody2D.mass = 0.2f;
                _jumpForce = 60;
            }
        }

        public void Update()
        {
            if (UIManager.Instance.IsPlayerAlive() && !UIManager.Instance.IsPaused())
            {
                if (_isSpaceLevel)
                    SpaceMovement();
                else
                    NormalMovement();
            }
        }

        private void SpaceMovement()
        {
            if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1"))
            {
                _rigidbody2D.AddForce(Vector2.up * _jumpForce);
                _jumpSound.Play();
            }
            if (Input.GetKeyDown("down") || Input.GetButtonDown("Fire2")) _rigidbody2D.AddForce(Vector2.down * 0.5f * _jumpForce);
            if (_rigidbody2D.velocity.y < -2)
                PlayerAnimations.PlayFallSpaceAnimation();
            else if (_rigidbody2D.velocity.y > 1)
                PlayerAnimations.PlayJumpSpaceAnimation();
            else
                PlayerAnimations.PlayIdleAnimation();
        }

        private void NormalMovement()
        {
            _onGround = CheckGround();
            if (_onGround)
                if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1"))
                {
                    _jumpSound.Play();
                    _rigidbody2D.AddForce(Vector2.up * _jumpForce);
                }
                else
                    PlayerAnimations.PlayIdleAnimation();
            //else
            //  if (Input.GetAxis("Vertical") < 0)
            //    _rigidbody2D.AddForce(Vector2.down * _jumpForce * 0.02f);
            HandleAir();
        }

        private bool CheckGround()
        {
            if (Physics2D.Raycast(transform.position, Vector3.down, 1f))
                return true;
            else
                return false;
        }

        public void HandleAir()
        {
            if (IsJumping())
                PlayerAnimations.PlayJumpAnimation(); 
            if (IsFalling())
                PlayerAnimations.PlayFallAnimation();
        }

        private bool IsFalling()
        {
            if (_rigidbody2D.velocity.y < 0 && !_onGround)
                return true;
            return false;
        }

        private bool IsJumping()
        {
            if (_rigidbody2D.velocity.y > 0 && !_onGround)
                return true;
            return false;
        }
    } 
}
