using Godot;

public partial class NPC : CharacterBody2D
{
    public NPCData Data { get; private set; }

    public void Initialize(NPCData data)
    {
        Data = data;
    }
}
