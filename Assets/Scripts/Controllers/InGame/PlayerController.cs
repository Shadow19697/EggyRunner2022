using Scripts.Managers;
using Scripts.Animations;
using UnityEngine;

namespace Scripts.Controllers.InGame
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private int _jumpForce;
        [SerializeField] private int _speedMove;
        [SerializeField] private GameObject _rig;

        private Rigidbody2D _rigidbody2d;
        private AnyStateAnimator _anyStateAnimator;
        private float _inputVertical;
        private bool _onGround;

        private void Start()
        {
            _rigidbody2d = GetComponent<Rigidbody2D>();
            _anyStateAnimator = _rig.GetComponent<AnyStateAnimator>();
            if (PlayerPrefsManager.GetLevelSelected() == 3)
                _rigidbody2d.gravityScale = 0;
            AnyStateAnimation[] animations = new AnyStateAnimation[]
            {
                new AnyStateAnimation("Idle"),
                new AnyStateAnimation("Jump"),
                new AnyStateAnimation("Fall")
            };
            _anyStateAnimator.AddAnimations(animations);
        }

        private void Update()
        {
            if (PlayerPrefsManager.GetLevelSelected() == 3)
                VerticalMove();
            else
            {
                _onGround = CheckGround();
                if (Input.GetButtonDown("Jump") && _onGround)
                {
                    _rigidbody2d.AddForce(Vector2.up * _jumpForce);
                }
                else
                    _anyStateAnimator.TryPlayAnimation("Idle");
                HandleAir();
            }
        }

        private void VerticalMove()
        {
            _inputVertical = Input.GetAxisRaw("Vertical");
            _anyStateAnimator.TryPlayAnimation("Idle");
            _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, _inputVertical * _speedMove);
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
            if (IsFalling())
                _anyStateAnimator.TryPlayAnimation("Fall");
            if (IsJumping())
                _anyStateAnimator.TryPlayAnimation("Jump");
        }

        private bool IsFalling()
        {
            if (_rigidbody2d.velocity.y < 0 && !_onGround)
                return true;
            return false;
        }

        private bool IsJumping()
        {
            if (_rigidbody2d.velocity.y > 0 && !_onGround)
                return true;
            return false;
        }
    }
}