using Scripts.Managers;
using Scripts.Managers.InGame;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerMovements : MonoBehaviour
    {
        [SerializeField] private int _jumpForce;
        [SerializeField] private int _speedMove;

        private Rigidbody2D _rigidbody2D;
        private float _inputVertical;
        private bool _onGround;


        public void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            if (PlayerPrefsManager.GetLevelSelected() == 3)
                _rigidbody2D.gravityScale = 0;
        }

        public void Update()
        {
            if (UIManager.Instance.GetLifesCount() != 0)
            {
                if (PlayerPrefsManager.GetLevelSelected() == 3)
                    VerticalMove();
                else
                {
                    _onGround = CheckGround();
                    if (Input.GetButtonDown("Jump") && _onGround)
                        _rigidbody2D.AddForce(Vector2.up * _jumpForce);
                    else
                        PlayerAnimations.PlayIdleAnimation();
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
