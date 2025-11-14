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

	private void HandleInput()
    {
        if(Input.IsActionPressed("move_up") && Input.IsActionPressed("move_down") ||
		   Input.IsActionPressed("move_left") && Input.IsActionJustPressed("move_right")) {
			return;
		   }
		if(Input.IsActionPressed("move_up"))
        {
            
        } else if(Input.IsActionPressed("move_right"))
        {
            
        } else if(Input.IsActionPressed("move_down"))
        {
            
        } else if(Input.IsActionPressed("move_left"))
        {
            
        }  
    }

	public override void _PhysicsProcess(double delta)
	{
		if(isMoving)
        {
            MoveStep(delta);
        } else
        {
            HandleInput();
        }
	}

    private void MoveStep(double delta)
    {
        
    }
}
