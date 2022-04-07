using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers.InGame
{
    public class ObstacleController : MonoBehaviour
    {
        private CapsuleCollider2D _capsuleCollider2D;

        private void Start()
        {
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }

        public CapsuleCollider2D GetCapsuleCollider()
        {
            return _capsuleCollider2D;
        }
    }
}
