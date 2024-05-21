using Godot;
using System;

public partial class UIManager : Node
{
	[ExportCategory("UIs")] 
	[Export]
	private PackedScene _hud;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		
	}

	public void AddHUD(Player player)
	{
		var current = this.GetChild(0);
		current.QueueFree();

		HUD hud = _hud.Instantiate<HUD>();
		hud.SetPlayer(player);
		this.AddChild(hud);

	}
	
}
