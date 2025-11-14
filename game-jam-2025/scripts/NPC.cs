using Godot;

public partial class NPC : CharacterBody2D
{
    public NPCData Data { get; private set; }

    public void Initialize(NPCData data)
    {
        Data = data;
    }

    public void Interact()
    {
        GD.Print($"{Data.FirstName} says: {Data.Dialogue[0][0]}");
		GD.Print($"{Data.FirstName} says: {Data.Dialogue[0][1]}");
    }
}
