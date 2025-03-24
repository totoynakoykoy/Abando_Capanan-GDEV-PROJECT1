using Godot;
using System;
using System.Collections.Generic;

public partial class Leaderboard : Control
{
    private VBoxContainer scoresContainer;
    private Button backButton;

    private static List<ScoreEntry> scores = new List<ScoreEntry>();

    public override void _Ready()
    {
        scoresContainer = GetNode<VBoxContainer>("VBoxContainer/ScoreContainer");
        backButton = GetNode<Button>("VBoxContainer/Back");

        backButton.Pressed += OnBackButtonPressed;

        UpdateLeaderboard();
    }

    private void OnBackButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Menu.tscn"); // Change to your menu scene path
    }

    public static void AddScore(string playerName, int score)
    {
        scores.Add(new ScoreEntry(playerName, score));
        scores.Sort((a, b) => b.Score.CompareTo(a.Score)); // Sort by highest score
    }

    private void UpdateLeaderboard()
    {
        foreach (Node child in scoresContainer.GetChildren())
        {
            scoresContainer.RemoveChild(child);
            child.QueueFree();
        }

        foreach (ScoreEntry entry in scores)
        {
            Label scoreLabel = new Label();
            scoreLabel.Text = $"{entry.PlayerName}: {entry.Score}";
            scoresContainer.AddChild(scoreLabel);
        }
    }

	public void _on_back_pressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Menu/Menu.tscn");
    }
}

public class ScoreEntry
{
    public string PlayerName;
    public int Score;

    public ScoreEntry(string playerName, int score)
    {
        PlayerName = playerName;
        Score = score;
    }
}

 
