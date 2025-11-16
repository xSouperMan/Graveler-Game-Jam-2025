using Godot;

public partial class Map : Node2D 
{
	
	private Timer _dayTimer;
	private Label _timeLabel;
	public Player player;
 

	public override void _Ready()
	{
		GD.Print("Level wurde geladen und ist bereit!");

		_dayTimer = GetNode<Timer>("DayTimer"); 
		_timeLabel = GetNode<Label>("CanvasLayer/TimeLabel");
		player = GetNode<Player>("Player");
		_dayTimer.Timeout += _OnDayTimerTimeout;
	}


	public override void _Process(double delta)
	{
		double remainingTime = _dayTimer.TimeLeft;
		
		int minutes = Mathf.FloorToInt(remainingTime / 60);
		int seconds = Mathf.FloorToInt(remainingTime % 60);

		_timeLabel.Text = $"{minutes:D2}:{seconds:D2}";
	}
	
	
	private void _OnDayTimerTimeout()
	{
		GD.Print("Der Tag ist zu Ende!");
		
		_timeLabel.Text = "Day over!"; 
		
		if(player.Quota > player.DeportedCount)
		{
			GetTree().ChangeSceneToFile("res://scenes/LoseScreen.tscn");
			return;
		}

		GetTree().ChangeSceneToFile("res://scenes/tages_zusammenfassung.tscn");
	}
}
