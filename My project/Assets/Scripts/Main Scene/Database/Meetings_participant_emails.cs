using Realms;
using MongoDB.Bson;
using System.Collections.Generic;

public partial class Meetings_participant_emails : IEmbeddedObject
{
    [MapTo("participant_email")]
    public string? ParticipantEmail { get; set; }

    public Meetings_participant_emails(string participantEmail) 
    {
        ParticipantEmail = participantEmail;
    }
}