using Godot;
using System;

public partial class InteractionUi : ColorRect
{
	[Export] public Button Deport;
	[Export] public Button ComingBack;
	[Export] public Button NoDeport;
	[Export] public NPC npc;
	[Export] public Player player;
	[Export] public TextureRect id;
	public override void _Ready()
	{
		Deport.Pressed += DeportEnemy;
		ComingBack.Pressed += MightComeBack;
		NoDeport.Pressed += NoDeportEnemy;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void DeportEnemy()
	{
		var rand = new Random();
		AudioManager.Instance.Call("PlaySpeech", rand.Next(0,3));
		if(npc == null)
		{
			return;
		}
		id.Visible = true;
		Visible = false;
		npc.PlayerArrest();
		player.inCreaseDeportedCount();
	}

	public void NoDeportEnemy()
	{
		var rand = new Random();
		AudioManager.Instance.Call("PlaySpeech", rand.Next(0,3));
		if(npc == null)
		{
			return;
		}
		id.Visible = true;
		Visible = false;
		npc.PlayerNotArrest();
	}

	private void MightComeBack()
	{
		var rand = new Random();
		AudioManager.Instance.Call("PlaySpeech", rand.Next(0,3));
		if(npc == null)
		{
			return;
		}
		id.Visible = true;
		Visible = false;
		npc.PlayerMightComeBack();
	}
}
