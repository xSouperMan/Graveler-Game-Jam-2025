using Godot;
using System;
using System.Collections.Generic;


public partial class NPCManager : Node
{

	//pool for random NPC attributes and dialogue options

	private string[] firstNames = { "Philip", "Anna", "Karl", "Lena", "Mark", "Sophia", "Luiz", "David", "Sofja", "Francois", "Igritt", "Ying", "Kazuma", "Mike", "Mohammed", "Fatima", "Julian", "Henrik", "Omar", "Arjun", "Mei", "Seung", "Amara", "Thiago", "Luciana", "Yara", "Kwame", "Zuri", "Levent", "Efe", "Rajesh", "Niran", "Soraya", "Jamal", "Farid", "Nadia", "Samira", "Ayaan", "Imani", "Darius", "Viktor", "Eliska", "Alina", "Radek", "Bogdan", "Milena", "Stjepan", "Nikos", "Elif", "Baran", "Catarina", "Renato", "Julieta", "Manuel", "Soledad", "Harper", "Jackson", "Keiko", "Takumi", "Bao", "Thuy", "Linh", "Amine", "Walid", "Yassin", "Helena", "Marek", "João"};
	private string[] lastNames = { "Müller", "Fischer", "Schmidt", "Klein", "Bauer", "Weber", "Hoffmann", "Wagner", "Becker", "Schneider", "De La Cruz", "Rodriguez", "Kowalski", "Nowak", "Horvat", "Garcia", "Lopez", "Gomez", "Santos", "Silva", "Rossi", "Ricci", "Bianchi", "Moretti", "Conti", "O'Connor", "Murphy", "Smith", "Johnson", "Brown", "Andersen", "Nielsen", "Larsen", "Johansson", "Svensson", "Novak", "Petrov", "Ivanov", "Popov", "Stoica", "Nguyen", "Yamada", "Tanaka", "Kimura", "Han", "Singh", "Sharma", "Kumar", "Ali", "Rahman", "Hassan", "Ibrahim", "Haddad", "Gharib", "Saleh", "Diallo", "Okafor", "Mensah", "Osei", "Adeyemi", "Dupont", "Dubois", "Lefevre", "Laurent", "Demir", "Yilmaz", "Papadopoulos", "van Dijk" };
	private string[] occupations = { "Merchant", "Guard", "Student", "Farmer", "Teacher", "Union leader", "Conspiracy theorist", "Priest", "Preacher"};
	private string[] addresses = { "West Gate", "Market Street", "Old Road", "Hauser Street", "Martin Street"};
	private bool criminalRecord;
	private Random rand = new Random();

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

	private string[] mightComeBackResponses =
    {
        "See you around.",
		"Allright.",
		"Get Lost.",
		"Have a good day.",
		"No need for that.",
		"Take care."
    };

	private NPCData generateRandomNPC()
	{

		string first = firstNames[rand.Next(firstNames.Length)];
		string last = lastNames[rand.Next(lastNames.Length)];

		int age = rand.Next(17,90);

		string occupation = occupations[rand.Next(occupations.Length)];
		string address = addresses[rand.Next(addresses.Length)];

		string idExpirationDate = $"{rand.Next(1, 32):00}.{rand.Next(1, 13):00}.{rand.Next(2025, 2035)}";

		string greeting = greetings[rand.Next(greetings.Length)];
		string arrestResponse = arrestResponses[rand.Next(arrestResponses.Length)];
		string notArrestResponse = notArrestResponses[rand.Next(notArrestResponses.Length)];
		string mightComeBackResponse = mightComeBackResponses[rand.Next(mightComeBackResponses.Length)];

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
			mightComeBackResponse,
			criminalRecord
		);
	}

	[Export] public PackedScene NpcScene;
	private int DailyNumberOfNPCs;
	public override void _Ready()
	{
		var NPCLocations = GetTree().GetNodesInGroup("NPC");
		DailyNumberOfNPCs = NPCLocations.Count;
		var dailyNPCs = new List<NPCData>();

		for(int i=0; i<DailyNumberOfNPCs; i++)
		{
			dailyNPCs.Add(generateRandomNPC());
		}

		SpawnNPCs(dailyNPCs, NPCLocations);
	}

	private void SpawnNPCs(List<NPCData> NPClist, Godot.Collections.Array<Node> NPCLocations)
	{

		if (NpcScene == null)
		{
			GD.PrintErr("NpcScene export is empty! NPCs couldn't be generated");
			return;
		}

		for(int i=0; i<NPClist.Count; i++){

			var npcInstance = NpcScene.Instantiate();

			NPC npcNode = npcInstance as NPC;

			if (npcNode == null)
			{
				GD.PrintErr($"Failed to get NPC script on instance {i}!");
			}

			npcNode.Initialize(NPClist[i], ((Node2D)NPCLocations[i]).GlobalPosition);

			AddChild(npcNode);
		}    
	}
}
