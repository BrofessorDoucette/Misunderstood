using Godot;
using System;

public partial class Bootstrap : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		InitializeSteam();
		
	}

	
	private void InitializeSteam()
	{
		Steam.SteamInit();
		
		if (!Steam.IsSteamRunning())
		{
			GD.PrintErr("Steam is not running, or you do not have the game installed!");
			GD.PrintErr("Quitting!");
			
			GetTree().Quit();

			return;

		}
		
		GD.Print("Steam is running with appID: " + Steam.GetAppID().ToString());
		GD.Print("Steam Name Found: " + Steam.GetPersonaName());
		GD.Print("Steam ID Found: " + Steam.GetSteamID());

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
