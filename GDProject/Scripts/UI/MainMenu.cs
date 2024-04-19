using Godot;
using System;

public partial class MainMenu : Control
{

	[ExportCategory("Hooks")] 
	[Export] 
	private Bootstrap _bootstrap;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	private void _on_single_player_pressed()
	{
		GD.Print("Single Player Pressed!");
		
	}
	
	private void _on_multiplayer_pressed()
	{
		_bootstrap.NetworkManager.InitializeSteamIfNot();
		_bootstrap.NetworkManager.CreateLobby();
		
		
	}
	
	private void _on_settings_pressed()
	{
		_bootstrap.NetworkManager.Rpc(nameof(_bootstrap.NetworkManager.RpcTest));

	}
}
