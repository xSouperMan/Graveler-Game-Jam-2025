using Godot;
using System.Collections.Generic;

public partial class NPCManager : Node
{
    [Export] public PackedScene NpcScene; // Assign NPC.tscn here

    private List<NPCData> npcDefinitions = new()
    {
        new NPCData(
            "Philip", "Fischer",
            new string[][] {
                new [] { "Howdy partner!" },
                new [] { "Nice weather today!" }
            },
            26,
            "Student",
            "Hauserstraße 16",
            "15.09.2025"
        ),
        new NPCData(
            "Anna", "Müller",
            new string[][] {
                new [] { "Hello there!" },
                new [] { "I love this town." }
            },
            23,
            "Merchant",
            "Marktstraße 8",
            "01.12.2027"
        ),
        new NPCData(
            "Karl", "Schmidt",
            new string[][] {
                new [] { "Greetings!" },
                new [] { "Stay safe out there." }
            },
            35,
            "Guard",
            "West Gate",
            "12.08.2029"
        ),
    };

    public override void _Ready()
    {
        if (NpcScene == null)
        {
            GD.PrintErr("NpcScene export is empty! Assign NPC.tscn to NPCManager node.");
            return;
        }

        int tile = 16; // tile size

        for (int i = 0; i < npcDefinitions.Count; i++)
        {
            // Instantiate the scene as Node
            var npcInstance = NpcScene.Instantiate();

            // Get the NPC script attached to the root node
            NPC npcNode = npcInstance as NPC;

            if (npcNode == null)
            {
                GD.PrintErr($"Failed to get NPC script on instance {i}!");
                continue;
            }

            // Assign NPC data
            npcNode.Initialize(npcDefinitions[i]);

            // Place NPC in the world (auto-spaced)
            npcNode.GlobalPosition = new Vector2(5 + i * 3, 3) * tile;

            // Add to scene tree
            AddChild(npcNode);
        }
    }
}
