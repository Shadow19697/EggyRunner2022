using Scripts.Managers.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Player
{
    public static class PlayerInteractions
    {
        public static void InteractionManager(string tagType, CapsuleCollider2D capsuleCollider2D)
        {
            switch (tagType)
            {
                case "Obstacle":
                    UIManager.Instance.UpdateLifesCount(-1);
                    break;
                case "UpgradeX2":
                    UIManager.Instance.UpdateScoreMultiplier(2);
                    break;
                case "UpgradeX3":
                    UIManager.Instance.UpdateScoreMultiplier(3);
                    break;
                case "Life":
                    UIManager.Instance.UpdateLifesCount(1);
                    break;
                case "Inmortality":
                    capsuleCollider2D.enabled = false;
                    break;
                default: break;
            }
        }
    } 
}
