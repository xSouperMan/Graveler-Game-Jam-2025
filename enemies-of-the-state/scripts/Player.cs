using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public int TILE_SIZE = 16;
	[Export] public float StepTime = 0.3f;
	[Export] public int Quota;
	[Export] public int DeportedCount;
	[Export] public TileMapLayer TileMapLayer1;
	[Export] public TileMapLayer TileMapLayer2;
	[Export] public TextureRect Paper;
	[Export] public TextureRect Mail;
	[Export] public TextureRect ID;
	[Export] public RichTextLabel FirstName;
	[Export] public RichTextLabel FamilyName;
	[Export] public RichTextLabel Occupation;
	[Export] public RichTextLabel Adress;
	[Export] public RichTextLabel CriminalRecord;
	[Export] public RichTextLabel Age;
	[Export] public RichTextLabel ExpirationDate;
	[Export] public InteractionUi InteractionUI;
	[Export] public Label DeportedCountDisplay;

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
		HandleKeyPress();
		if(_isMoving)
		{
			MoveStep((float)delta);
		} else
		{
			HandleInput();
		}
	}

	private void HandleKeyPress()
	{
		

		bool ePressed = Input.IsActionJustPressed("e");

		
		if(ePressed && Mail != null && Paper != null)
		{
			if(Mail.Visible)
			{
				Mail.Visible = false;
				Paper.Visible = true;
			} else
			{
				Mail.Visible = true;
				Paper.Visible = false;
			}
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

		bool interactionJust = Input.IsActionJustPressed("interaction");

		bool qPressed = Input.IsActionJustPressed("q");

		if(interactionJust)
		{
			HandleInteraction();
		}

		if(qPressed)
		{
			if(!InteractionUI.Visible)
			{
				var npc = GetNpcAt(GlobalPosition + _facingDir*TILE_SIZE);
				if(npc != null)
				{
					var data = npc.Data;
					FirstName.Text = data.FirstName; 
					FamilyName.Text = data.LastName;
					Occupation.Text = data.Occupation;
					Adress.Text = data.Address;
					CriminalRecord.Text = data.CriminalRecord ? "YES" : "NO";
					Age.Text = data.Age.ToString();
					ExpirationDate.Text = data.IdExpirationDate;

					ID.Visible = false;
					InteractionUI.Visible = true;
					InteractionUI.npc = npc;
				}
			} else
			{
				ID.Visible = true;
				InteractionUI.Visible = false;
			}
		}

		if(!CheckCanMove())
		{
			return;
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

	private bool CheckCanMove()
	{
		if(InteractionUI.Visible)
		{
			return false;
		}
		return true;
	}

	private bool CheckIfInteractable()
	{

		return false;
	}

	private void HandleInteraction()
	{
		Vector2 NPCpos = _startPos + _facingDir * TILE_SIZE;
		if (NpcAt(NPCpos))
		{
			GD.Print("interaction avaliable");

			var npcs = GetTree().GetNodesInGroup("NPCs");

			NPC currentNPC = new NPC();

			foreach (Node2D npcNode in npcs)
			{
				if (npcNode is NPC npc)
				{
					if (NPCpos.DistanceTo(npc.GlobalPosition) <= 1f)
					{
						currentNPC = npc;
						GD.Print("Found NPC: " + currentNPC.Data.FirstName);
					}
				}
				GD.Print("something went wrong");
			}

			currentNPC.Interact(VectorToInvertedDirection(_facingDir));
		}
	}

	private Direction VectorToInvertedDirection(Vector2 vec)
	{
		if (_facingDir == Vector2.Up)
		{
			return Direction.DOWN;
		}  
		else if (_facingDir == Vector2.Down)
		{
			return Direction.UP;
		}    
		else if (_facingDir == Vector2.Left)
		{
			return Direction.RIGHT;
		}  
		else
		{
			return Direction.LEFT;
		}          
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

	private NPC GetNpcAt(Vector2 targetWorldPos)
	{
		var npcs = GetTree().GetNodesInGroup("NPCs");

		foreach (var npc in npcs)
		{
			if(npc is NPC)
			{
				NPC _npc = (NPC) npc;
				if(targetWorldPos.DistanceTo(_npc.GlobalPosition) <= 1f)
				{
					return _npc;
				}
			}
		}
		return null;
	}

	internal void inCreaseDeportedCount()
	{
		DeportedCount++;
		DeportedCountDisplay.Text = "Deported Today: "+DeportedCount;
	}
}
