using Godot;


public partial class Game : Node2D
{   
    const double GEM_MARGIN = 50.0;

    private static readonly AudioStream EXPLODE_SOUND = 
                GD.Load<AudioStream>("res://assets/explode.wav");

    //[Export] private Gem _gem;
    [Export] private PackedScene _gemScene;
    [Export] private Timer _spawnTimer;
    [Export] private Label _scoreLabel;
    [Export] private AudioStreamPlayer _music;
    [Export] private AudioStreamPlayer2D _effects;
    //[Export] private AudioStream _explodeSound;
    //[Export] private NodePath _gemPath; // Gem
    //private Gem _gem;

    private int _score = 0;

    private Control GameOverScreen;
    private Label FinalScoreLabel;
    private Label ScoreLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        //Gem gem = GetNode<Gem>("Gem");
        //_gem = GetNode<Gem>(_gemPath);
        //_gem.OnScored += OnScored;
        _spawnTimer.Timeout += SpawnGem;
        SpawnGem();
       GameOverScreen = GetNodeOrNull<Control>("CanvasLayer/Control");
        FinalScoreLabel = GetNodeOrNull<Label>("CanvasLayer/Control/FinalScoreLabel");
        ScoreLabel = GetNodeOrNull<Label>("ScoreLabel");  // Adjust if necessary

        if (GameOverScreen == null)
            GD.PrintErr("GameOverScreen not found! Check if 'CanvasLayer/Control' exists.");
        if (FinalScoreLabel == null)
            GD.PrintErr("FinalScoreLabel not found! Check if 'CanvasLayer/Control/FinalScoreLabel' exists.");
        if (ScoreLabel == null)
            GD.PrintErr("ScoreLabel not found! Check if 'ScoreLabel' exists.");

        // Hide the Game Over UI at start
        if (GameOverScreen != null)
            GameOverScreen.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void OnGameOver()
    {
        GameOverScreen.Visible = true;
        FinalScoreLabel.Text = $"Final Score: {_score}";
    }


    private void SpawnGem()
    {
        Rect2 vpr = GetViewportRect();
        Gem gem = (Gem)_gemScene.Instantiate();

        AddChild(gem);

        float rX = (float)GD.RandRange(
            vpr.Position.X + GEM_MARGIN, vpr.End.X - GEM_MARGIN
        );

        gem.Position = new Vector2(rX, -100);
        gem.OnScored += OnScored;
        gem.OnGemOffScreen += GameOver;
    }

    private void OnScored()
    {
        GD.Print("OnScored Received");
        _score += 1;
        _scoreLabel.Text = $"{_score:0000}";
        _effects.Play();
    }

    private void GameOver()
    {
        GD.Print("GameOver");
        foreach (Node node in GetChildren())
        {
            node.SetProcess(false);
        }
        _spawnTimer.Stop();
        _music.Stop();

        _effects.Stop();
        _effects.Stream = EXPLODE_SOUND;
        _effects.Play();

        Leaderboard.AddScore("Player1", _score);

        GD.Print("Game Over triggered!");  // Debugging message

        if (GameOverScreen != null)
        {
            GameOverScreen.Visible = true;
            GD.Print("Game Over screen is now visible.");
        }
        else
        {
            GD.PrintErr("GameOverScreen is null! Check node assignment.");
        }

        if (FinalScoreLabel != null && ScoreLabel != null)
        {
            FinalScoreLabel.Text = "Final Score: " + ScoreLabel.Text;
        }

    }

    public void _on_menu_pressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Menu/Menu.tscn");
    }   

   
}
