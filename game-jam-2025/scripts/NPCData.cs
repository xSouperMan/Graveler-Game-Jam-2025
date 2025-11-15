using Godot;
using System;

public class NPCData
{
    public string FirstName { get; }
    public string LastName { get; }
    public int Age { get; }
    public string Occupation { get; }
    public string Address { get; }
    public string IdExpirationDate { get; }
    public string Greeting { get;}
    public string ArrestResponse {get;}
    public string NotArrestResponse {get;}
    public bool CriminalRecord {get;}

    public NPCData(
        string firstName,
        string lastName,
        int age,
        string occupation,
        string address,
        string idExpirationDate,
        string greeting,
        string arrestResponse,
        string notArrestResponse,
        bool criminalRecord
        )
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Occupation = occupation;
        Address = address;
        IdExpirationDate = idExpirationDate;
        Greeting = greeting;
        ArrestResponse = arrestResponse;
        NotArrestResponse = notArrestResponse;
        CriminalRecord = criminalRecord;
    }
}

