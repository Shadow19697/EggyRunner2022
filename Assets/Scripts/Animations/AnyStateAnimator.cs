using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Animations
{
    public class AnyStateAnimator : MonoBehaviour
    {
        private Animator animator;
        private Dictionary<string, AnyStateAnimation> animations = new Dictionary<string, AnyStateAnimation>();
        private string currentAnimation = string.Empty;

        private void Awake()
        {
            this.animator = GetComponent<Animator>();
        }

        public void AddAnimations(params AnyStateAnimation[] newAnimations)
        {
            for (int i = 0; i < newAnimations.Length; i++)
            {
                this.animations.Add(newAnimations[i].Name, newAnimations[i]);
            }
        }

        public void TryPlayAnimation(string newAnimation)
        {
            if (currentAnimation == "")
            {
                animations[newAnimation].Active = true;
                currentAnimation = newAnimation;
            }
            if (currentAnimation != newAnimation)
            {
                animations[currentAnimation].Active = false;
                animations[newAnimation].Active = true;
                currentAnimation = newAnimation;
            }
            Animate();
        }

        private void Animate()
        {
            foreach (string key in animations.Keys)
            {
                animator.SetBool(key, animations[key].Active);
            }
        }
    }
}