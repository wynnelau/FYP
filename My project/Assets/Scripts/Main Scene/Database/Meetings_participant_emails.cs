using System;
using System.Collections.Generic;
using Realms;
using MongoDB.Bson;
public partial class Meetings_participant_emails : IEmbeddedObject
{
    [MapTo("participant_email")]
    public string? ParticipantEmail { get; set; }
}