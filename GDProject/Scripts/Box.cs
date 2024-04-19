using Godot;

public partial class Box : CsgBox3D, INpc
{
	[ExportCategory("NPC")]
	[Export]
    public string CharacterName {get; private set;} = "Box";

	[Export]
	public float TotalHealth {get; private set;} = 100;

    public float Health {get; private set;} = 100;

	[Export]
	public INpc.RelationshipStatus Relationship {get; private set;} = INpc.RelationshipStatus.Neutral;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		Health = TotalHealth;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		
	}
}
