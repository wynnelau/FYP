using System;
using System.Collections.Generic;
using Realms;
using MongoDB.Bson;
public partial class MeetingAttendees_participant_emails : IEmbeddedObject
{
    [MapTo("participant_email")]
    public string? ParticipantEmail { get; set; }

    public MeetingAttendees_participant_emails(string participantEmail)
    {
        ParticipantEmail = participantEmail;
    }
}