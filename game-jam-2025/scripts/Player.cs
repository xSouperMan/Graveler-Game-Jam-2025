using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public int TILE_SIZE = 16;

	[Export] public float StepTime = 0.3f;

	private bool _isMoving;
	private Vector2 _startPos;
	private Vector2 _targetPos;
	private float _stepTimer = 0f;

	public override void _Ready()
	{
		GlobalPosition = GlobalPosition.Snapped(Vector2.One * TILE_SIZE);
	}

	public override void _PhysicsProcess(double delta)
	{
		if(_isMoving)
		{
			MoveStep((float)delta);
		} else
		{
			HandleInput();
		}
	}

	private void HandleInput()
	{
		Vector2 dir = Vector2.Zero;

		// Ignore opposite vertical inputs
		bool up = Input.IsActionPressed("move_up");
		bool down = Input.IsActionPressed("move_down");
		bool left = Input.IsActionPressed("move_left");
		bool right = Input.IsActionPressed("move_right");

		bool upJust = Input.IsActionJustPressed("move_up");
		bool downJust = Input.IsActionJustPressed("move_down");
		bool leftJust = Input.IsActionJustPressed("move_left");
		bool rightJust = Input.IsActionJustPressed("move_right");

		bool interaction = Input.IsActionPressed("interaction");
		bool interactionJust = Input.IsActionJustPressed("interaction");

		if(interaction || interactionJust)
		{
			HandleInteraction();
		}


		// Block vertical opposites
		if ((up || upJust) && !(down || downJust))
			dir = Vector2.Up;
		else if ((down || downJust) && !(up || upJust))
			dir = Vector2.Down;
		// Block horizontal opposites
		else if ((right || rightJust) && !(left || leftJust))
			dir = Vector2.Right;
		else if ((left || leftJust) && !(right || rightJust))
			dir = Vector2.Left;

		if (dir == Vector2.Zero)
		{
			_isMoving = false;
			return;
		}

		StartStep(dir);
	}

	private void HandleInteraction()
	{
		//TODO
	}

	private void StartStep(Vector2 dir)
	{
		_isMoving = true;
		_stepTimer = 0f;

		_startPos = GlobalPosition;
		_targetPos = _startPos + dir * TILE_SIZE;
	}

	private void MoveStep(float delta)
	{
		_stepTimer += delta;
		float t = Mathf.Clamp(_stepTimer / StepTime, 0f, 1f);

		GlobalPosition = _startPos.Lerp(_targetPos, t);

		if (t >= 1f)
		{
			GlobalPosition = _targetPos.Snapped(Vector2.One * TILE_SIZE);
			_startPos = GlobalPosition;

			TryContinueMovement();
		}
	}

	private void TryContinueMovement()
	{
		Vector2 dir = Vector2.Zero;

		// Simple pressed states (no JustPressed needed here)
		bool up = Input.IsActionPressed("move_up");
		bool down = Input.IsActionPressed("move_down");
		bool left = Input.IsActionPressed("move_left");
		bool right = Input.IsActionPressed("move_right");

		// Block vertical opposites
		if (up && !down)
			dir = Vector2.Up;
		else if (down && !up)
			dir = Vector2.Down;
		// Block horizontal opposites
		else if (right && !left)
			dir = Vector2.Right;
		else if (left && !right)
			dir = Vector2.Left;
		else
		{
			_isMoving = false;
			return;
		}

		_isMoving = true;
		_stepTimer = 0;
		_targetPos = _startPos + dir * TILE_SIZE;
	}
}
