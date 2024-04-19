using Godot;
using System;

public partial class SceneManager : Node
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	[Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void RpcTest()
	{
		GD.Print("The server pressed the settings button!");
	}
	
}
