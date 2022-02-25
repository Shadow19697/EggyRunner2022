using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerComponents
{
    [SerializeField] private Rigidbody2D rigidbody;

    public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }
}
