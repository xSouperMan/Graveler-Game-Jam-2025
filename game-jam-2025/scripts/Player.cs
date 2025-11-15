using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public int TILE_SIZE = 16;
	[Export] public float StepTime = 0.3f;
	[Export] public TileMapLayer TileMapLayer1;
	[Export] public TileMapLayer TileMapLayer2;

	private bool _isMoving;
	private Vector2 _startPos;
	private Vector2 _targetPos;
	private float _stepTimer = 0f;
	private AnimationPlayer anim;
	private Vector2 _facingDir = Vector2.Down;

	public override void _Ready()
	{
		GlobalPosition = GlobalPosition.Snapped(Vector2.One * TILE_SIZE);

		anim = GetNode<Sprite2D>("Sprite2D").GetNode<AnimationPlayer>("AnimationPlayer");
		UpdateAnimation(false);
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

	private void UpdateAnimation(bool isMoving)
	{
		if (anim == null)
		{
			return;
		}

		string prefix = isMoving ? "walk_" : "idle_";

		if (_facingDir == Vector2.Up)
		{
			anim.Play(prefix + "up");
		}
		else if (_facingDir == Vector2.Down)
		{
			anim.Play(prefix + "down");
		}
		else if (_facingDir == Vector2.Left)
		{
			anim.Play(prefix + "left");
		}
		else if (_facingDir == Vector2.Right)
		{
			anim.Play(prefix + "right");
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

		if (dir != Vector2.Zero)
		{
			_facingDir = dir;
		}

		if (dir == Vector2.Zero)
		{
			_isMoving = false;
			UpdateAnimation(false);
			return;
		}

		Vector2 candidate = GlobalPosition + dir * TILE_SIZE;
		if (!CanMoveTo(candidate))
		{
			_isMoving = false;
			UpdateAnimation(false);
			return;
		}

		StartStep(dir);
	}

	private bool CheckIfInteractable()
	{

        if (NpcAt(_startPos + _facingDir * TILE_SIZE))
        {
            GD.Print("interaction avaliable");
			return true;
        }

		return false;
	}

	private void HandleInteraction()
	{
		NpcAt(_facingDir);
	}

	private void StartStep(Vector2 dir)
	{
		_isMoving = true;
		_stepTimer = 0f;

		_startPos = GlobalPosition;
		_targetPos = _startPos + dir * TILE_SIZE;
		UpdateAnimation(true);
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
					if (t.GlobalPosition.DistanceTo(pos) < 1f)
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
			_isMoving = false;
			UpdateAnimation(false);
			return;
		}

		if (dir != Vector2.Zero)
		{
			_facingDir = dir;
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
		UpdateAnimation(true);
	}

	private bool CanMoveTo(Vector2 targetWorldPos)
	{
		if (TileMapLayer1 == null || TileMapLayer2 == null)
		{
			GD.Print("TML null");
			return true;
		}

		if(NpcAt(targetWorldPos))
		{
			return false;
		}

		Vector2 localPos1 = TileMapLayer1.ToLocal(targetWorldPos);
		Vector2I tileCoords1 = TileMapLayer1.LocalToMap(localPos1);
		Vector2 localPos2 = TileMapLayer2.ToLocal(targetWorldPos);
		Vector2I tileCoords2 = TileMapLayer2.LocalToMap(localPos2);

		var tileData1 = TileMapLayer1.GetCellTileData(tileCoords1);
		var tileData2 = TileMapLayer2.GetCellTileData(tileCoords2);
		if (tileData1 == null && tileData2 == null)
		{
			GD.Print("TD null");
			return false;
		}

		bool solid = false;

		if(tileData1 != null)
		{
			Variant v1 = tileData1.GetCustomData("solid");
			if (v1.VariantType != Variant.Type.Nil)
			{
				solid = (bool)v1;
				GD.Print(solid+"1");
			} 
		}
		
		if(tileData2 != null)
		{
			Variant v2 = tileData2.GetCustomData("solid");
			if (!solid && v2.VariantType != Variant.Type.Nil)
			{
				solid = (bool)v2;
				GD.Print(solid+"2");
			}
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
