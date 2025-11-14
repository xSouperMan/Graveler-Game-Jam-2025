using Godot;
using System;

public class NPCData
{
    public string FirstName { get; }
    public string LastName { get; }
    public string[][] Dialogue { get; }
    public int Age { get; }
    public string Occupation { get; }
    public string Address { get; }
    public string IdExpirationDate { get; }

    public NPCData(
        string firstName,
        string lastName,
        string[][] dialogue,
        int age,
        string occupation,
        string address,
        string idExpirationDate)
    {
        FirstName = firstName;
        LastName = lastName;
        Dialogue = dialogue;
        Age = age;
        Occupation = occupation;
        Address = address;
        IdExpirationDate = idExpirationDate;
    }
}

