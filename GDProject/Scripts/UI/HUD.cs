using Godot;
using System;
public partial class HUD : Control
{

	[ExportCategory("Player")]
	[Export]

	private Player _player;

	private RayCast3D _playerRaycast;


	[ExportCategory("Crosshairs")]
	[Export]
	private Label _crosshairText;

	[Export]
	private CompressedTexture2D _whiteCrosshair;

	[Export]
	private CompressedTexture2D _greenCrosshair;

	[Export]
	private CompressedTexture2D _redCrosshair;

	[Export]
	private TextureRect _crosshair;

	[ExportCategory("NPC Information")]
	[Export]
	private Label _npcName;

	[Export]
	private ProgressBar _npcHealthBar;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playerRaycast = _player.CameraRaycast;
		_crosshair.Texture = _whiteCrosshair;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		var collider = _playerRaycast.GetCollider();

		if(collider != null){

			if(collider is INpc)
			{
				var npc = (INpc) collider;

				_npcName.Visible = true;
				_npcName.Text = npc.CharacterName;

				switch(npc.Relationship)
				{
					case INpc.RelationshipStatus.Passive:
						_crosshair.Texture = _greenCrosshair;
						break;
					case INpc.RelationshipStatus.Neutral:
						_crosshair.Texture = _whiteCrosshair;
						break;
					case INpc.RelationshipStatus.Hostile:
						_crosshair.Texture = _redCrosshair;
						_npcHealthBar.Visible = true;
						_npcHealthBar.MaxValue = npc.TotalHealth;
						_npcHealthBar.Value  = npc.Health;
						break;
					default:
						_crosshair.Texture = _whiteCrosshair;
						break;
				}
			} else {

				_npcName.Visible = false;
				_npcHealthBar.Visible = false;

			}

			if (collider is IItem){

				var item = (IItem) collider;
				_crosshairText.Visible = true;
				_crosshairText.Text = item.ItemName;
			} else {
				_crosshairText.Visible = false;
			}
		} else {

			_crosshair.Texture = _whiteCrosshair;
			_crosshairText.Visible = false;
			_npcName.Visible = false;
			_npcHealthBar.Visible = false;
		}

	}

}
