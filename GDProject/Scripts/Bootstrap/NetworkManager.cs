using Godot;
using System;
using System.Text.RegularExpressions;

public partial class NetworkManager : Node
{
	public bool SteamIsInitialized;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SteamIsInitialized = false;
		InitializeSteamIfNot();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (SteamIsInitialized)
		{
			Steam.RunCallbacks();
		}	
	}

	private void SetupCallbacks()
	{
		GD.Print("Setting up callbacks!");

		Steam.JoinRequested += OnLobbyJoinRequested;
		Steam.LobbyJoined += OnLobbyJoined;
	}
	

	private void OnLobbyJoined(long lobby, long permissions, bool locked, long response)
	{
		if (Multiplayer.IsServer())
		{
			GD.Print("Created Lobby: " + lobby);
			
			Steam.SetLobbyJoinable((ulong) lobby, true);
			Steam.AllowP2PPacketRelay(true);
		}

		if (!Multiplayer.IsServer())
		{
			GD.Print("Joined Existing Lobby: " + lobby);
		}
		
	}

	private void OnLobbyJoinRequested(long lobbyid, long steamid)
	{
		String name = Steam.GetFriendPersonaName((ulong) steamid);
		GD.Print("Attempting to join " + name + "'s lobby: " + lobbyid);
		
		Multiplayer.MultiplayerPeer = null;
		
		var peer = new SteamMultiplayerPeer();
		peer.ConnectLobby((ulong) lobbyid);
		Multiplayer.MultiplayerPeer = peer;
	}

	private void RemoveCallbacks()
	{
		Steam.JoinRequested -= OnLobbyJoinRequested;
		Steam.LobbyJoined -= OnLobbyJoined;
	}

	public void CreateLobby()
	{
		var peer = new SteamMultiplayerPeer();
		peer.CreateLobby(SteamMultiplayerPeer.Lobby_type.FriendsOnly, 8);
		Multiplayer.MultiplayerPeer = peer;
	}
	
	
	public void ShutdownSteamIfRunning()
	{
		if (SteamIsInitialized)
		{
			GD.Print("Safely Shutting Down Steam!");
			Steam.SteamShutdown();
			SteamIsInitialized = false;
			RemoveCallbacks();
		}
	}
	
	public void InitializeSteamIfNot()
	{
		if (SteamIsInitialized)
		{
			GD.Print("Steam is already initialized!");
			return;
		}
		
		GD.Print("Initializing Steam!");
		
		var statusDictionary = Steam.SteamInit(true, 2329460);

		statusDictionary.TryGetValue("status", out Variant status);

		switch ((int) status)
		{
			case 1:
				GD.Print("Steam Successfully Initialized!");
				SteamIsInitialized = true;
				break;
			case 2:
				GD.PrintErr("Steam failed to initialize!");
				return;
			case 20:
				GD.PrintErr("Steam is not running!");
				return;
			case 79:
				GD.PrintErr("Invalid App ID");
				return;
		}
		
		
		GD.Print("Steam is now running with appID: " + Steam.GetAppID().ToString());
		GD.Print("Steam Name Found: " + Steam.GetPersonaName());
		GD.Print("Steam ID Found: " + Steam.GetSteamID());
		
		SetupCallbacks();

	}
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, TransferChannel = 0)]
	public void RpcTest()
	{
		GD.Print("The server pressed the settings button!");
	}
}
