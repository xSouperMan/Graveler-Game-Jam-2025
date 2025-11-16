using System.Security.AccessControl;
using Godot;

public partial class NPC : CharacterBody2D
{
	enum InteractionState{Greet, Await, Finished}
	private InteractionState state = InteractionState.Greet;
	public NPCData Data { get; private set; }
	private Direction dir;
	private AnimationPlayer anim;
	private TextureRect dialogBubble;
	private Label dialogLabel;

	public void Initialize(NPCData data, Vector2 globalPosition)
	{
		anim = GetNode<Sprite2D>("Sprite2D").GetNode<AnimationPlayer>("AnimationPlayer");
		Data = data;
		dir = Direction.DOWN;
		anim.Play("idle_down");
		GlobalPosition = globalPosition;
		GD.Print(GlobalPosition);
		//dialogBubble.Visible = false;
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
        AddToGroup("NPC");

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
		GD.Print($"{Data.FirstName}: {Data.ArrestResponse}");
		state = InteractionState.Finished;
	}

	public void PlayerNotArrest()
	{
		GD.Print($"{Data.FirstName}: {Data.NotArrestResponse}");
		state = InteractionState.Finished;
	}
}