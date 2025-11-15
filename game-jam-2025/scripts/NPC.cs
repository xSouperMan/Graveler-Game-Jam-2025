using Godot;

public partial class NPC : CharacterBody2D
{
    enum InteractionState{Greet, Await, Finished}
    private InteractionState state = InteractionState.Greet;
    public NPCData Data { get; private set; }

    public void Initialize(NPCData data)
    {
        Data = data;
    }

    public void Interact()
    {
        switch (state)
        {
            case InteractionState.Greet:
                GD.Print($"{Data.FirstName}: {Data.Greeting[0]}");
                state = InteractionState.Await;
                break;

            case InteractionState.Await:
                GD.Print($"{Data.FirstName} waits for your decision.");
                break;

            case InteractionState.Finished:
                GD.Print($"{Data.FirstName}: We already talked.");
                break;
        }
    }

    public void PlayerArrest()
    {
        GD.Print($"{Data.FirstName}: {Data.ArrestResponse}");
        state = InteractionState.Finished;
    }

    public void PlayerNotArrest()
    {
        GD.Print($"{Data.FirstName}: {Data.NotArrestResponse}");
        state = InteractionState.Finished;
    }
}
