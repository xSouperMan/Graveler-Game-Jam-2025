using Godot;
using System;

public partial class StartControl : Control
{
	[Export] Button back;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		back.Pressed += backPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("quit"))
		{
			GetTree().Quit();
		}
	}
	public void backPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/Start_Menu.tscn");
	}
}
