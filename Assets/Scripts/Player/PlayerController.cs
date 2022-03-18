using Scripts.Managers.InGame;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject _rig;

        private CapsuleCollider2D _capsuleCollider2D;
        private string _tagType;

        private void Start()
        {
            PlayerAnimations.Rig = _rig;
            PlayerAnimations.InitAnimations();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }

        private void Update()
        {
            if (UIManager.Instance.GetLifesCount() == 0)
            {
                _capsuleCollider2D.enabled = false;
                PlayerAnimations.PlayDeathAnimation();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _tagType = collision.gameObject.tag;
            PlayerInteractions.InteractionManager(_tagType, _capsuleCollider2D);
        }
    }
}