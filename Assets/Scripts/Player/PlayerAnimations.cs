using Scripts.Animations;
using UnityEngine;

namespace Scripts.Player
{
    public static class PlayerAnimations
    {
        
        private static AnyStateAnimator _anyStateAnimator;
        private static GameObject _rig;

        public static GameObject Rig { get => _rig; set => _rig = value; }

        public static void InitAnimations()
        {
            _anyStateAnimator = Rig.GetComponent<AnyStateAnimator>();
            AnyStateAnimation[] animations = new AnyStateAnimation[]
            {
                new AnyStateAnimation("Idle"),
                new AnyStateAnimation("Jump"),
                new AnyStateAnimation("Fall")
            };
            _anyStateAnimator.AddAnimations(animations);
        }

        public static void PlayIdleAnimation()
        {
            _anyStateAnimator.TryPlayAnimation("Idle");
        }
        public static void PlayJumpAnimation()
        {
            _anyStateAnimator.TryPlayAnimation("Jump");
        }
        public static void PlayFallAnimation()
        {
            _anyStateAnimator.TryPlayAnimation("Fall");
        }
        public static void PlayDeathAnimation()
        {

        }
    } 
}
