using Godot;
using System;

public partial class Bootstrap : Node
{

	[ExportCategory("Hooks")] 
	[Export] 
	public SceneManager SceneManager { get; private set; }
	[Export]
	public UIManager UiManager { get; private set; }	
	[Export]
	public NetworkManager NetworkManager { get; private set; }
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetTree().AutoAcceptQuit = false;

		
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _Notification(int what)
	{
		if (what == NotificationWMCloseRequest)
		{
			if (NetworkManager != null)
			{
				NetworkManager.ShutdownSteamIfRunning();
			}
			GetTree().Quit(); // default behavior
		}
	}
}
