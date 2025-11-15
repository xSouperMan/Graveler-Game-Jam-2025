using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public int TILE_SIZE = 16;
	[Export] public float StepTime = 0.3f;
	[Export] public TileMapLayer TileMapLayer;

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

		if((interaction || interactionJust) && CheckIfInteractable())
		{
			{
				HandleInteraction();
			}
		}


		if ((up || upJust) && !(down || downJust))
		{
			dir = Vector2.Up;
		}
		else if ((down || downJust) && !(up || upJust))
		{
			dir = Vector2.Down;
		}
		else if ((right || rightJust) && !(left || leftJust))
		{
			dir = Vector2.Right;
		}
		else if ((left || leftJust) && !(right || rightJust))
		{
			dir = Vector2.Left;
		}

		if (dir == Vector2.Zero)
		{
			{
				_isMoving = false;
				return;
			}
		}

		Vector2 candidate = GlobalPosition + dir * TILE_SIZE;
		if (!CanMoveTo(candidate))
		{
			{
				_isMoving = false;
				return;
			}
		}

		StartStep(dir);
	}

	private bool CheckIfInteractable()
	{
		//TODO

		return false;
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
			{
				GlobalPosition = _targetPos.Snapped(Vector2.One * TILE_SIZE);
				_startPos = GlobalPosition;

				var tp = GetTeleporterAt(GlobalPosition);
				if (tp != null)
					{
						if(tp.teleport_to == null) {
							return;
						}
						GD.Print(tp.teleport_to.GlobalPosition);
						GlobalPosition = tp.teleport_to.GlobalPosition;
						_isMoving = false;
						return;
					}

				TryContinueMovement();
			}
		}
	}

	private Teleporter GetTeleporterAt(Vector2 pos)
		{
			var teleporterNodes = GetTree().GetNodesInGroup("Teleporters");
			GD.Print(teleporterNodes.Count);
			foreach (Node node in teleporterNodes)
			{
				GD.Print(node);
				if (node is Teleporter t)
				{
					GD.Print(t.GlobalPosition);
					if (t.GlobalPosition.DistanceTo(pos) < 1f) // oder ==, wenn du exakt auf dem Tile vergleichst
						return t;
				}
			}

			return null;
		}

	private void TryContinueMovement()
	{
		Vector2 dir = Vector2.Zero;

		bool up = Input.IsActionPressed("move_up");
		bool down = Input.IsActionPressed("move_down");
		bool left = Input.IsActionPressed("move_left");
		bool right = Input.IsActionPressed("move_right");

		if (up && !down)
		{
			dir = Vector2.Up;
		}
		else if (down && !up)
		{
			dir = Vector2.Down;
		}
		else if (right && !left)
		{
			dir = Vector2.Right;
		}
		else if (left && !right)
		{
			dir = Vector2.Left;
		}
		else
		{
			{
				_isMoving = false;
				return;
			}
		}

		Vector2 candidate = _startPos + dir * TILE_SIZE;
		if (!CanMoveTo(candidate))
		{
			{
				_isMoving = false;
				return;
			}
		}

		_isMoving = true;
		_stepTimer = 0;
		_targetPos = candidate;
	}

	private bool CanMoveTo(Vector2 targetWorldPos)
	{
		if (TileMapLayer == null)
		{
			GD.Print("TML null");
			return true;
		}

		if(NpcAt(targetWorldPos))
		{
			return false;
		}

		Vector2 localPos = TileMapLayer.ToLocal(targetWorldPos);
		Vector2I tileCoords = TileMapLayer.LocalToMap(localPos);

		var tileData = TileMapLayer.GetCellTileData(tileCoords);
		if (tileData == null)
		{
			GD.Print("TD null");
			return false;
		}

		Variant v = tileData.GetCustomData("solid");

		bool solid = false;
		if (v.VariantType != Variant.Type.Nil)
		{
			solid = (bool)v;
		}
		return !solid;
	}

	private bool NpcAt(Vector2 targetWorldPos)
	{
		var npcs = GetTree().GetNodesInGroup("NPC");

		foreach (Node2D npc in npcs)
		{
			if(targetWorldPos.DistanceTo(npc.GlobalPosition) <= 1f)
			{
				return true;
			}
		}
		return false;
	}
}
