using Scripts.Animations;
using UnityEngine;

namespace Scripts.Player
{
    public static class PlayerAnimations
    {
        
        private static AnyStateAnimator _anyStateAnimator;
        private static GameObject _rig;
        private static ParticleSystem _explosion;
        private static ParticleSystem _deathAnimation;

        public static GameObject Rig { get => _rig; set => _rig = value; }
        public static ParticleSystem Explosion { get => _explosion; set => _explosion = value; }
        public static ParticleSystem DeathAnimation { get => _deathAnimation; set => _deathAnimation = value; }

        public static void InitAnimations()
        {
            _explosion.Pause();
            _deathAnimation.Pause();
            _anyStateAnimator = Rig.GetComponent<AnyStateAnimator>();
            AnyStateAnimation[] animations = new AnyStateAnimation[]
            {
                new AnyStateAnimation("Idle"),
                new AnyStateAnimation("Jump"),
                new AnyStateAnimation("Fall"),
                new AnyStateAnimation("FallSpace"),
                new AnyStateAnimation("JumpSpace"),
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
        public static void PlayFallSpaceAnimation()
        {
            _anyStateAnimator.TryPlayAnimation("FallSpace");
        }
        public static void PlayJumpSpaceAnimation()
        {
            _anyStateAnimator.TryPlayAnimation("JumpSpace");
        }
        public static void PlayDeathAnimation()
        {
            PlayExplosionAnimation();
            _deathAnimation.Play();
        }
        public static void PlayExplosionAnimation()
        {
            _explosion.Play();
        }
    } 
}
