public interface INpc
{
    public string CharacterName {get; }

    public float TotalHealth {get; }

    public float Health {get; }

    enum RelationshipStatus 
    {
        Passive, 
        Neutral,
        Hostile
    }

    public RelationshipStatus Relationship {get; }

}