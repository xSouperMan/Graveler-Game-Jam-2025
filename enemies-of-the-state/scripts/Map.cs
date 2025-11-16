using Godot;

// Erbt von deinem Root-Node-Typ (z.B. Node2D, Node3D, Node)
public partial class Map : Node2D 
{
	
	private Timer _dayTimer;
	private Label _timeLabel;


	public override void _Ready()
	{
		GD.Print("Level wurde geladen und ist bereit!");

		// === NEUER CODE IN _Ready() ===
		// Wir holen uns die Nodes über ihren Pfad und speichern sie
		// in den Variablen, die wir oben erstellt haben.

		// Holt den Timer-Node
		_dayTimer = GetNode<Timer>("DayTimer"); 
		
		// Holt das Label. WICHTIG: Der Pfad muss stimmen!
		// Wenn du den CanvasLayer "UI" genannt hast, schreibe "UI/TimeLabel"
		_timeLabel = GetNode<Label>("CanvasLayer/TimeLabel");
	}


	public override void _Process(double delta)
	{
		// === NEUER CODE IN _Process() ===
		// Diese Methode wird jeden Frame aufgerufen.
		// Perfekt, um unseren Timer-Text zu aktualisieren.

		// 1. Hole die verbleibende Zeit vom Timer
		// TimeLeft ist ein 'double'-Wert (z.B. 179.8, 179.7...)
		double remainingTime = _dayTimer.TimeLeft;

		// 2. Formatiere die Zeit in Minuten und Sekunden
		// Mathf.FloorToInt rundet auf die nächste ganze Zahl ab.
		int minutes = Mathf.FloorToInt(remainingTime / 60);
		int seconds = Mathf.FloorToInt(remainingTime % 60);

		// 3. Setze den Text des Labels
		// "{minutes:D2}:{seconds:D2}" ist ein Formatierungs-Trick:
		// "D2" bedeutet "Stelle als Zahl dar, aber immer mit 2 Ziffern".
		// Das macht aus "5:3" ein schönes "05:03".
		_timeLabel.Text = $"{minutes:D2}:{seconds:D2}";
	}
	
	
	// Diese Methode wird aufgerufen, wenn der Timer abläuft
	// (verbunden über den "Node"-Tab in Godot)
	private void _OnDayTimerTimeout()
	{
		GD.Print("Der Tag ist zu Ende!");
		
		// Verstecke den Timer-Text, wenn die Zeit abgelaufen ist
		_timeLabel.Text = "Tag vorbei!"; 
		// Oder: _timeLabel.Hide();

		// Wechsle die Szene
		GetTree().ChangeSceneToFile("res://scenes/tages_zusammenfassung.tscn");
	}
}
