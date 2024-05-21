using Godot;
using System;

public partial class SceneManager : Node
{
	[ExportCategory("Scenes")] 
	[Export]
	private PackedScene _map1;
	
	[Export]
	private PackedScene _player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public Player AddScene()
	{

		Player player = _player.Instantiate<Player>();
		this.AddChild(_player.Instantiate());
		this.AddChild(_map1.Instantiate());

		return player;
	}
	
	
	
	
}
