using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]private PlayerStats stats;
    [SerializeField] private PlayerComponents components;
    private PlayerReferences references;
    private PlayerUtilities utilities;
    private PlayerActions actions;

    public PlayerComponents Components { get => components; set => components = value; }
    public PlayerStats Stats { get => stats; set => stats = value; }

    private void Awake()
    {
        actions = new PlayerActions(this);
        utilities = new PlayerUtilities(this);
        stats.Speed = stats.WalkSpeed;
    }

    private void Update()
    {
        utilities.HandleInput();
    }

    private void FixedUpdate()
    {
        actions.Move(transform);
    }
}
