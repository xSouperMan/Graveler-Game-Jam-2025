using System;
using System.Security.AccessControl;
using Godot;

public partial class NPC : CharacterBody2D
{
	[Export] float DeportDelay = 2f;
	private Timer DeportTimer;
	enum InteractionState{Greet, Await, Finished}
	private InteractionState state = InteractionState.Greet;
	public NPCData Data { get; private set; }
	private Direction dir;
	private AnimationPlayer anim;
	private Control dialogBubble;
	private Label dialogLabel;

	public void Initialize(NPCData data, Vector2 globalPosition)
	{
		anim = GetNode<Sprite2D>("Sprite2D").GetNode<AnimationPlayer>("AnimationPlayer");
		Data = data;
		dir = Direction.DOWN;
		anim.Play("idle_down");
		GlobalPosition = globalPosition;
		GD.Print(GlobalPosition);
		dialogBubble = GetNode<Control>("Bubble");
		//dialogBubble.Visible = false;
		DeportTimer = GetNode<Timer>("Deport");
		DeportTimer.Timeout += Deport;
	}

    public void Interact(Direction direction)
	{

		switch(direction)
		{
			case Direction.DOWN:
				anim.Play("idle_down");
				break;

			case Direction.LEFT:
				anim.Play("idle_left");
				break;

			case Direction.UP:
				anim.Play("idle_up");
				break;

			case Direction.RIGHT:
				anim.Play("idle_right");
				break;
		}
		
		switch (state)
		{
			case InteractionState.Greet:
				GD.Print($"greeted {Data.FirstName}: {Data.Greeting}");
				ShowBubble(Data.Greeting);
				state = InteractionState.Await;
				break;

			case InteractionState.Await:
				GD.Print($"{Data.FirstName} waits for your decision.");
				break;

			case InteractionState.Finished:
				GD.Print($"{Data.FirstName}: We already talked.");
				break;
		}
	}

    public override void _Ready()
    {
        AddToGroup("NPCs");

		var dialogBubble = GetNode<Bubble>("Bubble");

		dialogLabel = dialogBubble.GetNode<Label>("TextureRect/Label");

		dialogBubble.Visible = false;
    }

	public void ShowBubble(string text)
	{
		var bubble = GetNode<Bubble>("Bubble");
		bubble.ShowMessage(text);

	}

	public void HideBubble()
	{
		dialogBubble.Visible = false;
	}

	public void PlayerArrest()
	{
		HideBubble();
		state = InteractionState.Finished;
		ShowBubble(Data.ArrestResponse);
		DeportTimer.Start(DeportDelay);
	}

	public void PlayerNotArrest()
	{
		HideBubble();
		state = InteractionState.Finished;
		ShowBubble(Data.NotArrestResponse);
	}

    internal void PlayerMightComeBack()
    {
		HideBubble();
		state = InteractionState.Finished;
		ShowBubble(Data.MightComeBackResponse);
    }

    private void Deport()
    {
        var npcs = GetTree().GetNodesInGroup("NPC");

		foreach (Node2D npc in npcs)
		{
			if(GlobalPosition.DistanceTo(npc.GlobalPosition) <= 1f)
			{
				npc.QueueFree();
				QueueFree();
				return;
			}
		}
    }
}