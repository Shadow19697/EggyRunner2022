using Scripts.Managers;
using Scripts.Managers.InGame;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerMovements : MonoBehaviour
    {
        [SerializeField] private int _jumpForce;
        [SerializeField] private int _speedMove;
        [SerializeField] private AudioSource _jumpSound;

        private Rigidbody2D _rigidbody2D;
        private float _inputVertical;
        private bool _onGround;
        private bool _isSpaceLevel;


        public void Start()
        {
            _jumpSound.Pause();
            if (PlayerPrefsManager.GetLevelSelected() == 3)
                _isSpaceLevel = true;
            else _isSpaceLevel = false;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            if (_isSpaceLevel)
                _rigidbody2D.gravityScale = 0;
        }

        public void Update()
        {
            if (UIManager.Instance.IsPlayerAlive() && !UIManager.Instance.IsPaused())
            {
                if (_isSpaceLevel)
                    VerticalMove();
                else
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
            }
        }

        private void VerticalMove()
        {
            _inputVertical = Input.GetAxisRaw("Vertical");
            PlayerAnimations.PlayIdleAnimation();
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _inputVertical * _speedMove);
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
