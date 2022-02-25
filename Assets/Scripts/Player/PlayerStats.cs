using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public float Speed { get; set; }

    public Vector2 Direction { get; set; }
    public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runkSpeed;
}
