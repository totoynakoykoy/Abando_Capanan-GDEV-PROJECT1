using Godot;
using System;

public partial class Menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TextureButton PlayButton = GetNode<TextureButton>("VBoxContainer/PlayButton");
   		PlayButton.Pressed += OnPlayButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// GD.Print("Play");
		// GetTree().ChangeSceneToFile("res://Scenes/Game/Game.tscn");
	}

	private void OnPlayButtonPressed()
	{
    	GD.Print("Play Button Pressed");
    	GetTree().ChangeSceneToFile("res://Scenes/Game/Game.tscn");
	}
}
