using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private const int TILE_SIZE = 16;
	private bool isMoving;

    public override void _Ready()
    {
        GlobalPosition = GlobalPosition.Snapped(Vector2.One * TILE_SIZE);
    }

	public override void _PhysicsProcess(double delta)
	{
		
	}
}
