using UnityEngine;

namespace Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject _rig;

        private void Start()
        {
            PlayerAnimations.Rig = _rig;
            PlayerAnimations.InitAnimations();
        }

        private void Update()
        {
            
        }
    }
}