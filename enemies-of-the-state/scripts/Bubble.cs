using Godot;
using System;

public partial class Bubble : Control
{
    [Export] public float Duration = 5.0f; // Sekunden
    private Label _label;
    private Timer _timer;

    public override void _Ready()
    {
        _label = GetNode<Label>("TextureRect/Label");
        _timer = GetNode<Timer>("Timer");

        _timer.Timeout += OnTimeout;
    }

    public void ShowMessage(string text)
    {
        _label.Text = text;
        Visible = true;

        // Timer starten
        _timer.Start(Duration);
    }

    private void OnTimeout()
    {
        Visible = false;
    }
}