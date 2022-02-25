using Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers.InGame
{
    public class PlayerController : MonoBehaviour
    {
        public int _jumpForce;
        public int _speedMove;

        private Rigidbody2D _rigidbody2d;
        private float _inputVertical;
        private bool _onGround;

        private void Start()
        {
            _rigidbody2d = GetComponent<Rigidbody2D>();
            if (PlayerPrefsManager.GetLevelSelected() == 3)
                _rigidbody2d.gravityScale = 0;
        }

        private void Update()
        {
            //Debug.DrawRay(transform.position, Vector3.down * 1f, Color.red);
            _onGround = CheckGround();
            if (PlayerPrefsManager.GetLevelSelected() == 3)
                VerticalMove();
            else
                if (Input.GetButtonDown("Jump") && _onGround)
                Jump();
        }

        private void FixedUpdate()
        {
            if (_inputVertical != 0)
                _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, _inputVertical * _speedMove);
        }

        private bool CheckGround()
        {
            if (Physics2D.Raycast(transform.position, Vector3.down, 1f))
                return true;
            else
                return false;
        }

        private void VerticalMove()
        {
            _inputVertical = Input.GetAxisRaw("Vertical");
        }

        private void Jump()
        {
            _rigidbody2d.AddForce(Vector2.up * _jumpForce);
        }

        
    }
}