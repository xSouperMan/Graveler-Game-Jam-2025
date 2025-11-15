using Godot;
using System;
using System.Collections.Generic;


public partial class NPCManager : Node
{

    //pool for random NPC attributes and dialogue options

    private string[] firstNames = { "Philip", "Anna", "Karl", "Lena", "Hans", "Sophia", "Luiz", "David", "Sofja", "Francois", "Igritt", "Ying"};
    private string[] lastNames = { "Müller", "Fischer", "Schmidt", "Klein", "Bauer", "De La Cruz", "Rodriguez", "O'Connor", "Kowalski", "Nguyen", "Smith", "Han"};

    private string[] occupations = { "Merchant", "Guard", "Student", "Farmer", "Teacher", "Union leader", "Conspiracy theorist", "Priest", "Preacher"};
    private string[] addresses = { "West Gate", "Market Street", "Old Road", "Hauser Street", "Martin Street"};
    private bool criminalRecord;

    private string[] greetings = {
        "Hello!",
        "Hey there!",
        "Good day!",
        "How’s it going?",
        "Lovely weather!",
        "Glory to Libertaria"
    };

    private string[] arrestResponses = {
        "What!? I didn’t do anything!",
        "Please, no!",
        "This is unjust!",
        "I surrender ...",
        "Please dont't ...  I have a family",
        "Screw you fascist pig!",
        "You can't ... I'm a loyal libertarian citizen"
    };

    private string[] notArrestResponses = {
        "Thank you!",
        "Have a nice day.",
        "You're kind.",
        "See you around!",
        "Glory to Libertaria",
        "Long live Libertaria"
    };

    private NPCData generateRandomNPC()
    {
        var rand = new Random();

        string first = firstNames[rand.Next(firstNames.Length)];
        string last = lastNames[rand.Next(lastNames.Length)];

        int age = rand.Next(17,80);

        string occupation = occupations[rand.Next(occupations.Length)];
        string address = addresses[rand.Next(addresses.Length)];

        string idExpirationDate = $"{rand.Next(1, 32):00}.{rand.Next(1, 13):00}.{rand.Next(2025, 2035)}";

        string greeting = greetings[rand.Next(greetings.Length)];
        string arrestResponse = arrestResponses[rand.Next(arrestResponses.Length)];
        string notArrestResponse = notArrestResponses[rand.Next(notArrestResponses.Length)];

        int randomCrimRec = rand.Next(1,6);
        bool criminalRecord = randomCrimRec == 1;

        return new NPCData(
            first,
            last,
            age,
            occupation,
            address,
            idExpirationDate,
            greeting,
            arrestResponse,
            notArrestResponse,
            criminalRecord
        );
    }

    [Export] public PackedScene NpcScene; // Assign NPC.tscn here
    [Export] public int DailyNumberOfNPCs = 20;
    public override void _Ready()
    {
        var dailyNPCs = new List<NPCData>();

        for(int i=0; i<DailyNumberOfNPCs; i++)
        {
            dailyNPCs.Add(generateRandomNPC());
        }

        SpawnNPCs(dailyNPCs);
    }

    private void SpawnNPCs(List<NPCData> NPClist){

        if (NpcScene == null)
        {
            GD.PrintErr("NpcScene export is empty! NPCs couldn't be generated");
            return;
        }

        int tile = 16; // tile size

        for(int i=0; i<NPClist.Count; i++){

            var rand = new Random();
            // Instantiate the scene as Node
            var npcInstance = NpcScene.Instantiate();

            // Get the NPC script attached to the root node
            NPC npcNode = npcInstance as NPC;

            if (npcNode == null)
            {
                GD.PrintErr($"Failed to get NPC script on instance {i}!");
            }

            // Assign NPC data
            npcNode.Initialize(NPClist[i]);

            // Place NPC in the world (auto-spaced)
            Vector2 positionVector = new Vector2(rand.Next(-8,8), rand.Next(-8,8)) * tile;
            npcNode.GlobalPosition = positionVector.Snapped(Vector2.One * tile);

            // Add to scene tree
            AddChild(npcNode);
        }    
    }
}
