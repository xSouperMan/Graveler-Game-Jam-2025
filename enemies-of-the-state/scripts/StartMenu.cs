using Godot;
using System;

public partial class StartMenu : Control
{
	[Export] Button start;
	[Export] Button controls;
	[Export] Button lore;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		start.Pressed += startPressed;
		controls.Pressed += controlsPressed;
		lore.Pressed += lorePressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("quit"))
		{
			GetTree().Quit();
		}
	}
	public void startPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/Map.tscn");
	}

	public void controlsPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/Start_Controls.tscn");
	}

	public void lorePressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/Start_Lore.tscn");
	}
}
